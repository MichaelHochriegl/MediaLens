using MediaLens.Models.ValueObjects;

namespace MediaLens.Models;

/// <summary>
/// Represents a text track within a media file.
/// </summary>
/// <param name="Format">The format of the text track.</param>
/// <param name="Language">The language associated with the text track.</param>
public sealed record TextTrack(
    string Format,
    Language? Language
);