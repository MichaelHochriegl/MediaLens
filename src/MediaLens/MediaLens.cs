using System.Globalization;
using MediaLens.Models;
using MediaLens.Models.ValueObjects;
using MediaLens.Native;

namespace MediaLens;

public sealed class MediaLens
{
    public MediaInfo Analyze(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Media file not found: {filePath}");
        }

        using var handle = MediaInfoHandle.Create();
        MediaInfoNative.Option(handle, "Language", "raw");

        if (MediaInfoNative.Open(handle, filePath) == 0)
            throw new InvalidOperationException($"Failed to open media file: {filePath}");

        try
        {
            return new MediaInfo(
                ParseGeneral(handle),
                ParseVideo(handle),
                ParseAudio(handle),
                ParseText(handle)
            );
        }
        finally
        {
            MediaInfoNative.Close(handle);
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

    private VideoTrack? ParseVideo(MediaInfoHandle handle)
    {
        if (GetStreamCount(handle, MediaInfoNative.StreamKind.Video) == 0)
            return null;

        return new VideoTrack(
            Format: GetString(handle, MediaInfoNative.StreamKind.Video, 0, "Format") ?? string.Empty,
            CodecId: GetString(handle, MediaInfoNative.StreamKind.Video, 0, "CodecID") ?? string.Empty,
            Language: GetString(handle, MediaInfoNative.StreamKind.Video, 0, "Language"),
            FrameRate: GetDouble(handle, MediaInfoNative.StreamKind.Video, 0, "FrameRate") is { } frameRate
                ? FrameRate.CreateOrNull(frameRate)
                : null,
            FrameRateMode: GetString(handle, MediaInfoNative.StreamKind.Video, 0, "FrameRate_Mode"),
            Width: GetInt(handle, MediaInfoNative.StreamKind.Video, 0, "Width"),
            Height: GetInt(handle, MediaInfoNative.StreamKind.Video, 0, "Height"),
            BitRate: GetDouble(handle, MediaInfoNative.StreamKind.Video, 0, "BitRate") is { } bitRate
                ? BitRate.CreateOrNull(bitRate)
                : null,
            AspectRatio: GetString(handle, MediaInfoNative.StreamKind.Video, 0, "DisplayAspectRatio")
        );
    }

    private AudioTrack[] ParseAudio(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Audio));

        if (count == 0)
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

    private TextTrack[] ParseText(MediaInfoHandle handle)
    {
        var count = Math.Max(0, GetStreamCount(handle, MediaInfoNative.StreamKind.Text));
        
        if (count == 0)
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