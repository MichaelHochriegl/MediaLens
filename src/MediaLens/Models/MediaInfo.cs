using System.Collections.Immutable;

namespace MediaLens.Models;

/// <summary>
/// Represents metadata extracted from a media file.
/// </summary>
/// <param name="General">The general metadata of the media file.</param>
/// <param name="VideoTracks">The video tracks of the media file.</param>
/// <param name="AudioTracks">The audio tracks of the media file.</param>
/// <param name="TextTracks">The text tracks of the media file.</param>
public sealed record MediaInfo(
    GeneralTrack General,
    ImmutableArray<VideoTrack> VideoTracks,
    ImmutableArray<AudioTrack> AudioTracks,
    ImmutableArray<TextTrack> TextTracks
);