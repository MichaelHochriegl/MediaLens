using MediaLens.Exceptions;
using MediaLens.Models;

namespace MediaLens;

/// <summary>
/// Provides operations for inspecting media files and retrieving metadata
/// about their general, video, audio, and text streams.
/// </summary>
public interface IMediaLens
{
    /// <summary>
    /// Inspects the specified media file and returns its extracted metadata.
    /// </summary>
    /// <param name="filePath">The path to the media file to inspect.</param>
    /// <returns>
    /// A <see cref="MediaInfo"/> instance containing metadata extracted from the media file.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="filePath"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="filePath"/> is empty or consists only of white-space characters.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the file specified by <paramref name="filePath"/> does not exist.
    /// </exception>
    /// <exception cref="MediaLensNativeDependencyException">
    /// Thrown when the required native MediaInfo dependency cannot be loaded.
    /// </exception>
    /// <exception cref="MediaLensHandleException">
    /// Thrown when the underlying native MediaInfo handle cannot be created or used.
    /// </exception>
    /// <exception cref="MediaLensOpenException">
    /// Thrown when the file exists but cannot be opened or parsed as media.
    /// </exception>
    /// <exception cref="MediaLensException">
    /// Thrown when an inspection error occurs.
    /// </exception>
    MediaInfo Inspect(string filePath);

    /// <summary>
    /// Attempts to inspect the specified media file.
    /// </summary>
    /// <param name="filePath">The path to the media file to inspect.</param>
    /// <param name="info">
    /// When this method returns, contains the extracted <see cref="MediaInfo"/> if the operation
    /// succeeds; otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the media file was successfully inspected; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="filePath"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="filePath"/> is empty or consists only of white-space characters.
    /// </exception>
    bool TryInspect(string filePath, out MediaInfo? info);
}