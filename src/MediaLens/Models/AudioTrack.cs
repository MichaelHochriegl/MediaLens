using MediaLens.Models.ValueObjects;

namespace MediaLens.Models;

/// <summary>
/// Represents an audio track within a media file.
/// </summary>
/// <param name="Format">The format of the audio track.</param>
/// <param name="CodecId">The codec identifier of the audio track.</param>
/// <param name="Language">The language associated with the audio track.</param>
/// <param name="Channels">The number of audio channels in the track.</param>
/// <param name="ChannelLayout">The channel layout of the audio track.</param>
/// <param name="SamplingRate">The sampling rate of the audio track.</param>
/// <param name="BitRate">The bit rate of the audio track.</param>
public sealed record AudioTrack(
    string Format,
    string CodecId,
    Language? Language,
    int? Channels,
    string? ChannelLayout,
    Frequency? SamplingRate,
    BitRate? BitRate
);