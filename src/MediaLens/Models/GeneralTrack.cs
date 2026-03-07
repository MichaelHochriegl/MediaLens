using MediaLens.Models.ValueObjects;

namespace MediaLens.Models;

/// <summary>
/// Represents general metadata about a media file.
/// </summary>
/// <param name="FileName">The name of the media file.</param>
/// <param name="Format">The container or format of the media file.</param>
/// <param name="Duration">The duration of the media file.</param>
/// <param name="FileSize">The size of the media file.</param>
/// <param name="OverallBitRateMode">The overall bit rate mode of the media file.</param>
/// <param name="OverallBitRate">The overall bit rate of the media file.</param>
public sealed record GeneralTrack(
    string FileName,
    string Format,
    TimeSpan? Duration,
    ByteSize? FileSize,
    string? OverallBitRateMode,
    BitRate? OverallBitRate
);