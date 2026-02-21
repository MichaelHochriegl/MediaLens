namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a frequency in <b>Hertz</b> (Hz).
/// </summary>
/// <remarks>Valid values are finite numbers greater than or equal to 0.</remarks>
public readonly record struct Frequency : IComparable<Frequency>
{
    /// <summary>Gets the frequency value in <b>Hertz</b> (Hz).</summary>
    public double Hertz { get; }

    private Frequency(double hertz) => Hertz = hertz;

    public static Frequency Create(double hertz)
    {
        if (double.IsNaN(hertz) || double.IsInfinity(hertz))
            throw new ArgumentOutOfRangeException(nameof(hertz), "Frequency must be a finite number.");

        if (hertz < 0)
            throw new ArgumentOutOfRangeException(nameof(hertz), "Frequency cannot be negative.");

        return new Frequency(hertz);
    }

    public static Frequency? CreateOrNull(double? hertz)
        => hertz is { } hz &&
           !double.IsNaN(hz) &&
           !double.IsInfinity(hz) &&
           hz >= 0
            ? new Frequency(hz)
            : null;

    public double Kilohertz => Hertz / 1000.0;

    public int CompareTo(Frequency other) => Hertz.CompareTo(other.Hertz);

    public static bool operator <(Frequency left, Frequency right) => left.CompareTo(right) < 0;
    public static bool operator >(Frequency left, Frequency right) => left.CompareTo(right) > 0;
    public static bool operator <=(Frequency left, Frequency right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Frequency left, Frequency right) => left.CompareTo(right) >= 0;

    public override string ToString() => $"{Kilohertz:0.###} kHz";
}