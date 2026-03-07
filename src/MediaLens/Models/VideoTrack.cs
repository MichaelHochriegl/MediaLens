using MediaLens.Models.ValueObjects;

namespace MediaLens.Models;

/// <summary>
/// Represents a video track within a media file.
/// </summary>
/// <param name="Format">The format of the video track.</param>
/// <param name="CodecId">The codec identifier of the video track.</param>
/// <param name="Language">The language associated with the video track.</param>
/// <param name="FrameRate">The frame rate of the video track.</param>
/// <param name="FrameRateMode">The frame rate mode of the video track.</param>
/// <param name="Width">The width of the video track in pixels.</param>
/// <param name="Height">The height of the video track in pixels.</param>
/// <param name="BitRate">The bit rate of the video track.</param>
/// <param name="AspectRatio">The display aspect ratio of the video track.</param>
public sealed record VideoTrack(
    string Format,
    string CodecId,
    Language? Language,
    FrameRate? FrameRate,
    string? FrameRateMode,
    int? Width,
    int? Height,
    BitRate? BitRate,
    string? AspectRatio
);