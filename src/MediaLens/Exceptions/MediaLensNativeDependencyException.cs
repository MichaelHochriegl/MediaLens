namespace MediaLens.Exceptions;

/// <summary>
/// Thrown when the native MediaInfo library cannot be loaded / initialized.
/// </summary>
public sealed class MediaLensNativeDependencyException : MediaLensException
{
    public MediaLensNativeDependencyException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}