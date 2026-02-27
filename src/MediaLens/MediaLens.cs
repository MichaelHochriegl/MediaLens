using System.Globalization;
using MediaLens.Models;
using MediaLens.Models.ValueObjects;
using MediaLens.Native;

namespace MediaLens;

public sealed class MediaLens : IDisposable
{
    private readonly MediaInfoHandle _handle;
    private bool _disposed;

    public MediaLens()
    {
        _handle = MediaInfoHandle.Create();

        MediaInfoNative.Option(_handle, "Language", "raw");
    }

    public MediaInfo Analyze(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Media file not found: {filePath}");
        }
        
        if (MediaInfoNative.Open(_handle, filePath) == 0)
            throw new InvalidOperationException($"Failed to open media file: {filePath}");

        try
        {
            return new MediaInfo(
                ParseGeneral(),
                ParseVideo(),
                ParseAudio(),
                ParseText()
            );
        }
        finally
        {
            MediaInfoNative.Close(_handle);
        }
    }

    private GeneralTrack ParseGeneral()
        => new(
            FileName: GetString(MediaInfoNative.StreamKind.General, 0, "FileName") ?? string.Empty,
            Format: GetString(MediaInfoNative.StreamKind.General, 0, "Format") ?? string.Empty,
            Duration: GetTimeSpan(MediaInfoNative.StreamKind.General, 0, "Duration"),
            FileSize: GetLong(MediaInfoNative.StreamKind.General, 0, "FileSize") is { } fileSizeBytes ? ByteSize.CreateOrNull(fileSizeBytes) : null,
            OverallBitRateMode: GetString(MediaInfoNative.StreamKind.General, 0, "OverallBitRate_Mode"),
            OverallBitRate: GetDouble(MediaInfoNative.StreamKind.General, 0, "OverallBitRate") is {} overallBitRate ? BitRate.CreateOrNull(overallBitRate) : null
        );

    private VideoTrack? ParseVideo()
    {
        if (GetStreamCount(MediaInfoNative.StreamKind.Video) == 0)
            return null;

        return new VideoTrack(
            Format: GetString(MediaInfoNative.StreamKind.Video, 0, "Format") ?? string.Empty,
            CodecId: GetString(MediaInfoNative.StreamKind.Video, 0, "CodecID") ?? string.Empty,
            Language: GetString(MediaInfoNative.StreamKind.Video, 0, "Language"),
            FrameRate: GetDouble(MediaInfoNative.StreamKind.Video, 0, "FrameRate") is {} frameRate ? FrameRate.CreateOrNull(frameRate) : null,
            FrameRateMode: GetString(MediaInfoNative.StreamKind.Video, 0, "FrameRate_Mode"),
            Width: GetInt(MediaInfoNative.StreamKind.Video, 0, "Width"),
            Height: GetInt(MediaInfoNative.StreamKind.Video, 0, "Height"),
            BitRate: GetDouble(MediaInfoNative.StreamKind.Video, 0, "BitRate") is {} bitRate ? BitRate.CreateOrNull(bitRate) : null,
            AspectRatio: GetString(MediaInfoNative.StreamKind.Video, 0, "DisplayAspectRatio")
        );
    }

    private AudioTrack[] ParseAudio()
    {
        var count = GetStreamCount(MediaInfoNative.StreamKind.Audio);
        var list = new List<AudioTrack>(count);

        for (int i = 0; i < count; i++)
        {
            list.Add(new AudioTrack(
                Format: GetString(MediaInfoNative.StreamKind.Audio, i, "Format") ?? string.Empty,
                CodecId: GetString(MediaInfoNative.StreamKind.Audio, i, "CodecID") ?? string.Empty,
                Language: GetString(MediaInfoNative.StreamKind.Audio, i, "Language"),
                Channels: GetInt(MediaInfoNative.StreamKind.Audio, i, "Channels"),
                ChannelLayout: GetString(MediaInfoNative.StreamKind.Audio, i, "ChannelLayout"),
                SamplingRate: GetDouble(MediaInfoNative.StreamKind.Audio, i, "SamplingRate") is {} samplingRate ? Frequency.CreateOrNull(samplingRate) : null,
                BitRate: GetDouble(MediaInfoNative.StreamKind.Audio, i, "BitRate") is {} bitRate ? BitRate.CreateOrNull(bitRate) : null
            ));
        }

        return list.ToArray();
    }

    private TextTrack[] ParseText()
    {
        var count = GetStreamCount(MediaInfoNative.StreamKind.Text);
        var list = new List<TextTrack>(count);

        for (int i = 0; i < count; i++)
        {
            list.Add(new TextTrack(
                Format: GetString(MediaInfoNative.StreamKind.Text, i, "Format") ?? string.Empty,
                Language: GetString(MediaInfoNative.StreamKind.Text, i, "Language")
            ));
        }

        return list.ToArray();
    }

    private int GetStreamCount(MediaInfoNative.StreamKind kind)
        => MediaInfoNative.CountGet(_handle, kind, -1);

    private string? GetString(MediaInfoNative.StreamKind kind, int index, string name)
    {
        var ptr = MediaInfoNative.Get(
            _handle,
            kind,
            index,
            name,
            MediaInfoNative.InfoKind.Text,
            MediaInfoNative.InfoKind.Name);

        var value = MediaInfoNative.PtrToString(ptr);
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private int? GetInt(MediaInfoNative.StreamKind kind, int index, string name)
        => int.TryParse(GetString(kind, index, name),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;

    private long? GetLong(MediaInfoNative.StreamKind kind, int index, string name)
        => long.TryParse(GetString(kind, index, name),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;

    private double? GetDouble(MediaInfoNative.StreamKind kind, int index, string name)
        => double.TryParse(GetString(kind, index, name),
            NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;

    private TimeSpan? GetTimeSpan(MediaInfoNative.StreamKind kind, int index, string name)
        => GetDouble(kind, index, name) is { } ms
            ? TimeSpan.FromMilliseconds(ms)
            : null;

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _handle.Dispose();
    }
}
