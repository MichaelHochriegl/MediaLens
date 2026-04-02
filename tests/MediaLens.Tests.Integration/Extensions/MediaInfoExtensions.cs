using MediaLens.Models;

namespace MediaLens.Tests.Integration.Extensions;

/// <summary>
/// Provides extension methods for working with media information in the context of integration testing.
/// </summary>
/// <remarks>
/// This static class contains utility methods that extend the functionality of media-related operations,
/// facilitating streamlined processing and validation during integration tests.
/// </remarks>
public static class MediaInfoExtensions
{
    /// <summary>
    /// Provides extension methods for the MediaInfo type
    /// </summary>
    extension(MediaInfo info)
    {
        /// <summary>
        /// Converts the current instance of <see cref="MediaInfo"/> into a snapshot model.
        /// </summary>
        /// <remarks>
        /// Unfortunately, <c>Verify</c> doesn't properly handle <c>ImmutableArray</c> and will report empty arrays as <c>null</c>.
        /// This conversion mitigates that.
        /// </remarks>
        /// <returns>
        /// An object containing the general metadata and arrays of video, audio, and text tracks
        /// from the current <see cref="MediaInfo"/> instance.
        /// </returns>
        public object ToSnapshotModel() => new
        {
            info.General,
            VideoTracks = info.VideoTracks.ToArray(),
            AudioTracks = info.AudioTracks.ToArray(),
            TextTracks = info.TextTracks.ToArray(),
        };
    }
}