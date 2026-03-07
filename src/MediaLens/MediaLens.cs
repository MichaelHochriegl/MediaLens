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

            if (MediaInfoNative.Open(handle, filePath) == 0)
            {
                throw new MediaLensOpenException(filePath, "Failed to open the media file.");
            }

            try
            {
                return new MediaInfo(
                    ParseGeneral(handle),
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

    private GeneralTrack ParseGeneral(MediaInfoHandle handle)
        => new(
            FileName: GetString(handle, MediaInfoNative.StreamKind.General, 0, "FileName") ?? string.Empty,
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

    private VideoTrack[] ParseVideoTracks(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Video));

        if (count <= 0)
        {
            return [];
        }

        var result = new VideoTrack[count];

        for (int i = 0; i < count; i++)
        {
            result[i] = new VideoTrack(
                Format: GetString(handle, MediaInfoNative.StreamKind.Video, i, "Format") ?? string.Empty,
                CodecId: GetString(handle, MediaInfoNative.StreamKind.Video, i, "CodecID") ?? string.Empty,
                Language: GetString(handle, MediaInfoNative.StreamKind.Video, i, "Language"),
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
        }

        return result;
    }

    private AudioTrack[] ParseAudioTracks(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Audio));

        if (count <= 0)
        {
            return [];
        }

        var result = new AudioTrack[count];

        for (var i = 0; i < count; i++)
        {
            result[i] = new AudioTrack(
                Format: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "Format") ?? string.Empty,
                CodecId: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "CodecID") ?? string.Empty,
                Language: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "Language"),
                Channels: GetInt(handle, MediaInfoNative.StreamKind.Audio, i, "Channels"),
                ChannelLayout: GetString(handle, MediaInfoNative.StreamKind.Audio, i, "ChannelLayout"),
                SamplingRate: GetDouble(handle, MediaInfoNative.StreamKind.Audio, i, "SamplingRate") is { } samplingRate
                    ? Frequency.CreateOrNull(samplingRate)
                    : null,
                BitRate: GetDouble(handle, MediaInfoNative.StreamKind.Audio, i, "BitRate") is { } bitRate
                    ? BitRate.CreateOrNull(bitRate)
                    : null
            );
        }

        return result;
    }

    private TextTrack[] ParseTextTracks(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Text));

        if (count <= 0)
        {
            return [];
        }

        var result = new TextTrack[count];

        for (var i = 0; i < count; i++)
        {
            result[i] = new TextTrack(
                Format: GetString(handle, MediaInfoNative.StreamKind.Text, i, "Format") ?? string.Empty,
                Language: GetString(handle, MediaInfoNative.StreamKind.Text, i, "Language")
            );
        }

        return result;
    }

    private int GetStreamCount(MediaInfoHandle handle, MediaInfoNative.StreamKind kind)
        => MediaInfoNative.CountGet(handle, kind, -1);

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