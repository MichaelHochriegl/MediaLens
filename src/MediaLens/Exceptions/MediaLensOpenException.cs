namespace MediaLens.Exceptions;

/// <summary>
/// Thrown when a media file exists but cannot be opened/parsed as media.
/// </summary>
public sealed class MediaLensOpenException : MediaLensException
{
    public string FilePath { get; }

    public MediaLensOpenException(string filePath, string message, Exception? innerException = null)
        : base(message, innerException)
        => FilePath = filePath;
}