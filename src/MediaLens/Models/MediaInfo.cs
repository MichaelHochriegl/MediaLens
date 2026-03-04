namespace MediaLens.Models;

public sealed record MediaInfo(
    GeneralTrack General,
    IReadOnlyList<VideoTrack> VideoTracks,
    IReadOnlyList<AudioTrack> AudioTracks,
    IReadOnlyList<TextTrack> TextTracks
);