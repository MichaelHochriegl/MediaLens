namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a frame rate in <b>frames per second</b> (fps).
/// </summary>
/// <remarks>Valid values are finite numbers strictly greater than 0.</remarks>
public readonly record struct FrameRate : IComparable<FrameRate>
{
    /// <summary>
    /// Gets the frame rate in <b>frames per second</b> (fps).
    /// </summary>
    public double FramesPerSecond { get; }

    private FrameRate(double framesPerSecond) => FramesPerSecond = framesPerSecond;

    /// <summary>
    /// Creates a <see cref="FrameRate"/> from the specified value in <b>frames per second</b> (fps).
    /// </summary>
    /// <param name="framesPerSecond">The frame rate value in frames per second.</param>
    /// <returns>A <see cref="FrameRate"/> representing the specified value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="framesPerSecond"/> is not a finite number or is less than or equal to 0.
    /// </exception>
    public static FrameRate Create(double framesPerSecond)
    {
        if (double.IsNaN(framesPerSecond) || double.IsInfinity(framesPerSecond))
            throw new ArgumentOutOfRangeException(nameof(framesPerSecond), "Frame rate must be a finite number.");

        if (framesPerSecond <= 0)
            throw new ArgumentOutOfRangeException(nameof(framesPerSecond), "Frame rate must be greater than 0.");

        return new FrameRate(framesPerSecond);
    }

    /// <summary>
    /// Creates a <see cref="FrameRate"/> from the specified value in <b>frames per second</b> (fps),
    /// or returns <see langword="null"/> if the value is invalid.
    /// </summary>
    /// <param name="framesPerSecond">The frame rate value in frames per second.</param>
    /// <returns>
    /// A <see cref="FrameRate"/> representing the specified value, or <see langword="null"/>
    /// if <paramref name="framesPerSecond"/> is <see langword="null"/>, not finite, or less than or equal to 0.
    /// </returns>
    public static FrameRate? CreateOrNull(double? framesPerSecond)
        => framesPerSecond is { } fps &&
           !double.IsNaN(fps) &&
           !double.IsInfinity(fps) &&
           fps > 0
            ? new FrameRate(fps)
            : null;

    /// <summary>
    /// Compares the current value with another <see cref="FrameRate"/>.
    /// </summary>
    /// <param name="other">The value to compare with the current value.</param>
    /// <returns>
    /// A value less than 0 if this instance is less than <paramref name="other"/>,
    /// 0 if they are equal, or a value greater than 0 if this instance is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(FrameRate other) => FramesPerSecond.CompareTo(other.FramesPerSecond);

    public static bool operator <(FrameRate left, FrameRate right) => left.CompareTo(right) < 0;
    public static bool operator >(FrameRate left, FrameRate right) => left.CompareTo(right) > 0;
    public static bool operator <=(FrameRate left, FrameRate right) => left.CompareTo(right) <= 0;
    public static bool operator >=(FrameRate left, FrameRate right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Returns the string representation of the current frame rate.
    /// </summary>
    /// <returns>A string formatted in <b>frames per second</b> (fps).</returns>
    public override string ToString() => $"{FramesPerSecond:0.###} fps";
}