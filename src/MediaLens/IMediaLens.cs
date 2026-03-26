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

    /// <summary>
    /// Asynchronously inspects media data from the specified stream and returns its extracted metadata.
    /// </summary>
    /// <remarks>
    /// The stream is read sequentially and is not modified except for its position if it supports seeking.
    /// If <paramref name="fileName"/> is provided, it may be used as a fallback for metadata fields such as
    /// the general file name when the underlying native library cannot determine one from the stream alone.
    /// </remarks>
    /// <param name="stream">The stream containing the media data to inspect.</param>
    /// <param name="fileName">
    /// An optional file name or display name associated with the stream. This is not required for parsing,
    /// but may be used as fallback metadata when the native library does not provide one.
    /// </param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. When complete, the task result contains a
    /// <see cref="MediaInfo"/> instance with metadata extracted from the stream.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="stream"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="stream"/> is not readable.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Thrown when the stream does not support the operations required for inspection.
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
    /// Thrown when the stream cannot be opened or parsed as media.
    /// </exception>
    /// <exception cref="MediaLensException">
    /// Thrown when an inspection error occurs that is specific to MediaLens.
    /// </exception>
    // Task<MediaInfo> InspectAsync(Stream stream, string? fileName = null, CancellationToken ct = default);
}