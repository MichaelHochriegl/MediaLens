using MediaLens.Exceptions;
using MediaLens.Models;

namespace MediaLens;

/// <summary>
/// Provides operations for inspecting media sources and retrieving metadata
/// about their general, video, audio, and text streams.
/// </summary>
public interface IMediaLens
{
    /// <summary>
    /// Asynchronously inspects the media file at the specified path and returns its extracted metadata.
    /// </summary>
    /// <remarks>
    /// This method opens the file using the file system and will use a stream-based native loading path
    /// internally to support file names containing special characters.
    /// </remarks>
    /// <param name="filePath">The path to the media file to inspect.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. When complete, the task result contains a
    /// <see cref="MediaInfo"/> instance with metadata extracted from the media file.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="filePath"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="filePath"/> is empty or consists only of white-space characters.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// Thrown when the file specified by <paramref name="filePath"/> does not exist.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Thrown when the operation is canceled via <paramref name="ct"/>.
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
    /// Thrown when an inspection error occurs that is specific to MediaLens.
    /// </exception>
    Task<MediaInfo> InspectAsync(string filePath, CancellationToken ct = default);
}