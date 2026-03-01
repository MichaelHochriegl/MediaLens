namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a frame rate in <b>frames per second</b> (fps).
/// </summary>
/// <remarks>Valid values are finite numbers strictly greater than 0.</remarks>
public readonly record struct FrameRate : IComparable<FrameRate>
{
    /// <summary>Gets the frame rate in <b>frames per second</b> (fps).</summary>
    public double FramesPerSecond { get; }

    private FrameRate(double framesPerSecond) => FramesPerSecond = framesPerSecond;

    public static FrameRate Create(double framesPerSecond)
    {
        if (double.IsNaN(framesPerSecond) || double.IsInfinity(framesPerSecond))
            throw new ArgumentOutOfRangeException(nameof(framesPerSecond), "Frame rate must be a finite number.");

        if (framesPerSecond <= 0)
            throw new ArgumentOutOfRangeException(nameof(framesPerSecond), "Frame rate must be greater than 0.");

        return new FrameRate(framesPerSecond);
    }

    public static FrameRate? CreateOrNull(double? framesPerSecond)
        => framesPerSecond is { } fps &&
           !double.IsNaN(fps) &&
           !double.IsInfinity(fps) &&
           fps > 0
            ? new FrameRate(fps)
            : null;

    public int CompareTo(FrameRate other) => FramesPerSecond.CompareTo(other.FramesPerSecond);

    public static bool operator <(FrameRate left, FrameRate right) => left.CompareTo(right) < 0;
    public static bool operator >(FrameRate left, FrameRate right) => left.CompareTo(right) > 0;
    public static bool operator <=(FrameRate left, FrameRate right) => left.CompareTo(right) <= 0;
    public static bool operator >=(FrameRate left, FrameRate right) => left.CompareTo(right) >= 0;

    public override string ToString() => $"{FramesPerSecond:0.###} fps";
}