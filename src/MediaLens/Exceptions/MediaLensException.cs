namespace MediaLens.Exceptions;

/// <summary>
/// Base exception for MediaLens failures (excluding argument/IO exceptions thrown by the BCL).
/// </summary>
public class MediaLensException : Exception
{
    public MediaLensException() { }
    public MediaLensException(string message) : base(message) { }
    public MediaLensException(string message, Exception? innerException) : base(message, innerException) { }
}