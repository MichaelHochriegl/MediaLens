namespace MediaLens.Models;

public sealed record MediaInfo(
    GeneralTrack General,
    VideoTrack? Video,
    AudioTrack[] Audio,
    TextTrack[] Text
);