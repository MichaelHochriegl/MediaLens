namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a frequency in <b>Hertz</b> (Hz).
/// </summary>
/// <remarks>Valid values are finite numbers greater than or equal to 0.</remarks>
public readonly record struct Frequency : IComparable<Frequency>
{
    /// <summary>
    /// Gets the frequency value in <b>Hertz</b> (Hz).
    /// </summary>
    public double Hertz { get; }

    private Frequency(double hertz) => Hertz = hertz;

    /// <summary>
    /// Creates a <see cref="Frequency"/> from the specified value in <b>Hertz</b> (Hz).
    /// </summary>
    /// <param name="hertz">The frequency value in Hertz.</param>
    /// <returns>A <see cref="Frequency"/> representing the specified value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="hertz"/> is not a finite number or is less than 0.
    /// </exception>
    public static Frequency Create(double hertz)
    {
        if (double.IsNaN(hertz) || double.IsInfinity(hertz))
            throw new ArgumentOutOfRangeException(nameof(hertz), "Frequency must be a finite number.");

        if (hertz < 0)
            throw new ArgumentOutOfRangeException(nameof(hertz), "Frequency cannot be negative.");

        return new Frequency(hertz);
    }

    /// <summary>
    /// Creates a <see cref="Frequency"/> from the specified value in <b>Hertz</b> (Hz),
    /// or returns <see langword="null"/> if the value is invalid.
    /// </summary>
    /// <param name="hertz">The frequency value in Hertz.</param>
    /// <returns>
    /// A <see cref="Frequency"/> representing the specified value, or <see langword="null"/>
    /// if <paramref name="hertz"/> is <see langword="null"/>, not finite, or less than 0.
    /// </returns>
    public static Frequency? CreateOrNull(double? hertz)
        => hertz is { } hz &&
           !double.IsNaN(hz) &&
           !double.IsInfinity(hz) &&
           hz >= 0
            ? new Frequency(hz)
            : null;

    /// <summary>
    /// Gets the frequency in <b>kilohertz</b> (kHz).
    /// </summary>
    public double Kilohertz => Hertz / 1000.0;

    /// <summary>
    /// Compares the current value with another <see cref="Frequency"/>.
    /// </summary>
    /// <param name="other">The value to compare with the current value.</param>
    /// <returns>
    /// A value less than 0 if this instance is less than <paramref name="other"/>,
    /// 0 if they are equal, or a value greater than 0 if this instance is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(Frequency other) => Hertz.CompareTo(other.Hertz);

    public static bool operator <(Frequency left, Frequency right) => left.CompareTo(right) < 0;
    public static bool operator >(Frequency left, Frequency right) => left.CompareTo(right) > 0;
    public static bool operator <=(Frequency left, Frequency right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Frequency left, Frequency right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Returns the string representation of the current frequency.
    /// </summary>
    /// <returns>A string formatted in <b>kilohertz</b> (kHz).</returns>
    public override string ToString() => $"{Kilohertz:0.###} kHz";
}