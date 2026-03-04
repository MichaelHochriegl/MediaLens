using MediaLens.Exceptions;
using MediaLens.Models;

namespace MediaLens;

/// <summary>
/// Represents an abstraction for analyzing media files and extracting detailed metadata.
/// Provides operations to inspect media files and retrieve relevant information about
/// their general, video, audio, and text tracks.
/// </summary>
public interface IMediaLens
{
    /// <summary>
    /// Inspects a media file and retrieves detailed information about its content,
    /// including general properties, video tracks, audio tracks, and text tracks.
    /// </summary>
    /// <param name="filePath">
    /// The full file path of the media file to inspect.
    /// </param>
    /// <returns>
    /// A <see cref="Models.MediaInfo"/> instance containing metadata and track details
    /// extracted from the media file.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="filePath"/> is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="filePath"/> is an empty or whitespace string.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Thrown if the media file specified by <paramref name="filePath"/> does not exist.
    /// </exception>
    /// <exception cref="Exceptions.MediaLensNativeDependencyException">
    /// Thrown if there is an issue loading the native MediaInfo dependency required for processing.
    /// </exception>
    /// <exception cref="Exceptions.MediaLensHandleException">
    /// Thrown if there is an issue creating or using the native MediaInfo handle.
    /// </exception>
    /// <exception cref="Exceptions.MediaLensOpenException">
    /// Thrown if there is an issue opening or parsing the media file.
    /// </exception>
    /// <exception cref="Exceptions.MediaLensException">
    /// Thrown if an error occurs while inspecting the media file.
    /// </exception>
    MediaInfo Inspect(string filePath);

    /// <summary>
    /// Attempts to inspect the media file at the specified file path and retrieve its metadata.
    /// </summary>
    /// <param name="filePath">The path to the media file to inspect.</param>
    /// <param name="info">
    /// When this method returns, contains the <see cref="MediaInfo"/> metadata for the media file
    /// if the operation was successful; otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the media file was successfully inspected; otherwise, <c>false</c>.
    /// </returns>
    bool TryInspect(string filePath, out MediaInfo? info);
}