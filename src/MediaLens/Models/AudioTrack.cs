using MediaLens.Models.ValueObjects;

namespace MediaLens.Models;

public sealed record AudioTrack(
    string Format,
    string CodecId,
    string? Language,
    int? Channels,
    string? ChannelLayout,
    Frequency? SamplingRate,
    BitRate? BitRate
);