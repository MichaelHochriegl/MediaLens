using System.Collections.Immutable;
using System.Globalization;
using MediaLens.Exceptions;
using MediaLens.Models;
using MediaLens.Models.ValueObjects;
using MediaLens.Native;

namespace MediaLens;

public sealed class MediaLens : IMediaLens
{
    public MediaInfo Inspect(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Media file not found.", filePath);
        }

        MediaInfoHandle handle;
        try
        {
            handle = MediaInfoNative.New();
        }
        catch (Exception ex) when (ex is DllNotFoundException or BadImageFormatException or EntryPointNotFoundException)
        {
            throw new MediaLensNativeDependencyException(
                "Failed to load the native MediaInfo dependency. Ensure the correct native library is available for the current OS/architecture.",
                ex);
        }

        using (handle)
        {
            if (handle.IsInvalid)
            {
                throw new MediaLensHandleException("Failed to create a native MediaInfo handle.");
            }

            MediaInfoNative.Option(handle, "Language", "raw");

            if (!TryOpen(handle, filePath))
            {
                throw new MediaLensOpenException(filePath, "Failed to open the media file.");
            }

            try
            {
                return new MediaInfo(
                    ParseGeneral(handle, filePath),
                    ParseVideoTracks(handle),
                    ParseAudioTracks(handle),
                    ParseTextTracks(handle)
                );
            }
            finally
            {
                MediaInfoNative.Close(handle);
            }
        }
    }

    public bool TryInspect(string filePath, out MediaInfo? info)
    {
        try
        {
            info = Inspect(filePath);
            return true;
        }
        catch (Exception ex) when (ex is FileNotFoundException or MediaLensException)
        {
            info = null;
            return false;
        }
    }

    private static bool TryOpen(MediaInfoHandle handle, string filePath)
    {
        if (MediaInfoNative.Open(handle, filePath) != 0)
        {
            return true;
        }

        return !OperatingSystem.IsWindows() && TryOpenWithStream(handle, filePath);
    }

    private static bool TryOpenWithStream(MediaInfoHandle handle, string filePath)
    {
        const int bufferSize = 64 * 1024;
        var buffer = new byte[bufferSize];

        using var stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize,
            FileOptions.SequentialScan);

        MediaInfoNative.OpenBufferInit(handle, (ulong)stream.Length, 0);

        while (true)
        {
            var read = stream.Read(buffer, 0, buffer.Length);
            if (read <= 0)
            {
                break;
            }

            if (!MediaInfoNative.OpenBufferContinue(handle, buffer, (nuint)read))
            {
                return false;
            }

            var goTo = MediaInfoNative.OpenBufferContinueGoToGet(handle);
            if (goTo != ulong.MaxValue)
            {
                stream.Seek((long)goTo, SeekOrigin.Begin);
            }
        }

        return MediaInfoNative.OpenBufferFinalize(handle);
    }

    private GeneralTrack ParseGeneral(MediaInfoHandle handle, string filePath)
        => new(
            FileName: GetString(handle, MediaInfoNative.StreamKind.General, 0, "FileName") ?? Path.GetFileNameWithoutExtension(filePath),
            Format: GetString(handle, MediaInfoNative.StreamKind.General, 0, "Format") ?? string.Empty,
            Duration: GetTimeSpan(handle, MediaInfoNative.StreamKind.General, 0, "Duration"),
            FileSize: GetLong(handle, MediaInfoNative.StreamKind.General, 0, "FileSize") is { } fileSizeBytes
                ? ByteSize.CreateOrNull(fileSizeBytes)
                : null,
            OverallBitRateMode: GetString(handle, MediaInfoNative.StreamKind.General, 0, "OverallBitRate_Mode"),
            OverallBitRate: GetDouble(handle, MediaInfoNative.StreamKind.General, 0, "OverallBitRate") is
                { } overallBitRate
                ? BitRate.CreateOrNull(overallBitRate)
                : null
        );

    private ImmutableArray<VideoTrack> ParseVideoTracks(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Video));

        if (count <= 0)
        {
            return [];
        }

        var builder = ImmutableArray.CreateBuilder<VideoTrack>(count);

        for (var i = 0; i < count; i++)
        {
            var track = new VideoTrack(
                Format: GetString(handle, MediaInfoNative.StreamKind.Video, i, "Format") ?? string.Empty,
                CodecId: GetString(handle, MediaInfoNative.StreamKind.Video, i, "CodecID") ?? string.Empty,
                Language: GetString(handle, MediaInfoNative.StreamKind.Video, i, "Language") is { } language
                    ? Language.CreateOrNull(language)
                    : null,
                FrameRate: GetDouble(handle, MediaInfoNative.StreamKind.Video, i, "FrameRate") is { } frameRate
                    ? FrameRate.CreateOrNull(frameRate)
                    : null,
                FrameRateMode: GetString(handle, MediaInfoNative.StreamKind.Video, i, "FrameRate_Mode"),
                Width: GetInt(handle, MediaInfoNative.StreamKind.Video, i, "Width"),
                Height: GetInt(handle, MediaInfoNative.StreamKind.Video, i, "Height"),
                BitRate: GetDouble(handle, MediaInfoNative.StreamKind.Video, i, "BitRate") is { } bitRate
                    ? BitRate.CreateOrNull(bitRate)
                    : null,
                AspectRatio: GetString(handle, MediaInfoNative.StreamKind.Video, i, "DisplayAspectRatio")
            );

            builder.Add(track);
        }

        return builder.ToImmutable();
    }

    private ImmutableArray<AudioTrack> ParseAudioTracks(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Audio));

        if (count <= 0)
        {
            return [];
        }

        var builder = ImmutableArray.CreateBuilder<AudioTrack>(count);

        for (var i = 0; i < count; i++)
        {
            var track = new AudioTrack(
                Format: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "Format") ?? string.Empty,
                CodecId: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "CodecID") ?? string.Empty,
                Language: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "Language") is { } language
                    ? Language.CreateOrNull(language)
                    : null,
                Channels: GetInt(handle, MediaInfoNative.StreamKind.Audio, i, "Channels"),
                ChannelLayout: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "ChannelLayout"),
                SamplingRate: GetDouble(handle, MediaInfoNative.StreamKind.Audio, i, "SamplingRate") is { } samplingRate
                    ? Frequency.CreateOrNull(samplingRate)
                    : null,
                BitRate: GetDouble(handle, MediaInfoNative.StreamKind.Audio, i, "BitRate") is { } bitRate
                    ? BitRate.CreateOrNull(bitRate)
                    : null
            );

            builder.Add(track);
        }

        return builder.ToImmutable();
    }

    private ImmutableArray<TextTrack> ParseTextTracks(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Text));

        if (count <= 0)
        {
            return [];
        }

        var builder = ImmutableArray.CreateBuilder<TextTrack>(count);

        for (var i = 0; i < count; i++)
        {
            var track = new TextTrack(
                Format: GetString(handle, MediaInfoNative.StreamKind.Text, i, "Format") ?? string.Empty,
                Language: GetString(handle, MediaInfoNative.StreamKind.Text, i, "Language") is { } language
                    ? Language.CreateOrNull(language)
                    : null
            );

            builder.Add(track);
        }

        return builder.ToImmutable();
    }

    private int GetStreamCount(MediaInfoHandle handle, MediaInfoNative.StreamKind kind)
        => MediaInfoNative.CountGet(handle, kind, nuint.MaxValue);

    private string? GetString(MediaInfoHandle handle, MediaInfoNative.StreamKind kind, int index, string name)
    {
        var ptr = MediaInfoNative.Get(
            handle,
            kind,
            index,
            name,
            MediaInfoNative.InfoKind.Text,
            MediaInfoNative.InfoKind.Name);

        var value = MediaInfoNative.PtrToString(ptr);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private int? GetInt(MediaInfoHandle handle, MediaInfoNative.StreamKind kind, int index, string name)
        => int.TryParse(GetString(handle, kind, index, name),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var v)
            ? v
            : null;

    private long? GetLong(MediaInfoHandle handle, MediaInfoNative.StreamKind kind, int index, string name)
        => long.TryParse(GetString(handle, kind, index, name),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var v)
            ? v
            : null;

    private double? GetDouble(MediaInfoHandle handle, MediaInfoNative.StreamKind kind, int index, string name)
        => double.TryParse(GetString(handle, kind, index, name),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var v)
            ? v
            : null;

    private TimeSpan? GetTimeSpan(MediaInfoHandle handle, MediaInfoNative.StreamKind kind, int index, string name)
        => GetDouble(handle, kind, index, name) is { } ms
            ? TimeSpan.FromMilliseconds(ms)
            : null;
}