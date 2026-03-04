namespace MediaLens.Models;

public sealed record MediaInfo(
    GeneralTrack General,
    IReadOnlyList<VideoTrack> Videos,
    IReadOnlyList<AudioTrack> Audio,
    IReadOnlyList<TextTrack> Text
);