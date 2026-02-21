using MediaLens.Models.ValueObjects;

namespace MediaLens.Models;

public sealed record GeneralTrack(
    string FileName,
    string Format,
    TimeSpan? Duration,
    ByteSize? FileSize,
    string? OverallBitRateMode,
    BitRate? OverallBitRate
);