using MediaLens.Models.ValueObjects;

namespace MediaLens.Models;

public sealed record VideoTrack(
    string Format,
    string CodecId,
    string? Language,
    FrameRate? FrameRate,
    string? FrameRateMode,
    int? Width,
    int? Height,
    BitRate? BitRate,
    string? AspectRatio
);