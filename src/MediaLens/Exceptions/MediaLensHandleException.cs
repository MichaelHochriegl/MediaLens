namespace MediaLens.Exceptions;

/// <summary>
/// Thrown when MediaLens cannot create or use the underlying native handle.
/// </summary>
public sealed class MediaLensHandleException : MediaLensException
{
    public MediaLensHandleException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}