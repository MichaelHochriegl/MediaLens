namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a bit rate in <b>bits per second</b> (b/s).
/// </summary>
/// <remarks>Valid values are finite numbers greater than or equal to 0.</remarks>
public readonly record struct BitRate : IComparable<BitRate>
{
    /// <summary>
    /// Gets the bit rate value in <b>bits per second</b> (b/s).
    /// </summary>
    public double BitsPerSecond { get; }

    private BitRate(double bitsPerSecond) => BitsPerSecond = bitsPerSecond;

    /// <summary>
    /// Creates a <see cref="BitRate"/> from the specified value in <b>bits per second</b> (b/s).
    /// </summary>
    /// <param name="bitsPerSecond">The bit rate value in bits per second.</param>
    /// <returns>A <see cref="BitRate"/> representing the specified value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="bitsPerSecond"/> is not a finite number or is less than 0.
    /// </exception>
    public static BitRate Create(double bitsPerSecond)
    {
        if (double.IsNaN(bitsPerSecond) || double.IsInfinity(bitsPerSecond) || bitsPerSecond < 0)
            throw new ArgumentOutOfRangeException(nameof(bitsPerSecond), "Bitrate must be a finite number greater than or equal to 0.");

        return new BitRate(bitsPerSecond);
    }

    /// <summary>
    /// Creates a <see cref="BitRate"/> from the specified value in <b>bits per second</b> (b/s),
    /// or returns <see langword="null"/> if the value is invalid.
    /// </summary>
    /// <param name="bitsPerSecond">The bit rate value in bits per second.</param>
    /// <returns>
    /// A <see cref="BitRate"/> representing the specified value, or <see langword="null"/>
    /// if <paramref name="bitsPerSecond"/> is <see langword="null"/>, not finite, or less than 0.
    /// </returns>
    public static BitRate? CreateOrNull(double? bitsPerSecond)
        => bitsPerSecond is { } bps &&
           !double.IsNaN(bps) &&
           !double.IsInfinity(bps) &&
           bps >= 0
            ? new BitRate(bps)
            : null;

    /// <summary>
    /// Gets the bit rate in <b>kilobits per second</b> (kb/s), using SI units
    /// where 1 kb/s = 1000 b/s.
    /// </summary>
    public double KilobitsPerSecond => BitsPerSecond / 1000.0;

    /// <summary>
    /// Compares the current value with another <see cref="BitRate"/>.
    /// </summary>
    /// <param name="other">The value to compare with the current value.</param>
    /// <returns>
    /// A value less than 0 if this instance is less than <paramref name="other"/>,
    /// 0 if they are equal, or a value greater than 0 if this instance is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(BitRate other) => BitsPerSecond.CompareTo(other.BitsPerSecond);

    public static bool operator <(BitRate left, BitRate right) => left.CompareTo(right) < 0;
    public static bool operator >(BitRate left, BitRate right) => left.CompareTo(right) > 0;
    public static bool operator <=(BitRate left, BitRate right) => left.CompareTo(right) <= 0;
    public static bool operator >=(BitRate left, BitRate right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Returns the string representation of the current bit rate.
    /// </summary>
    /// <returns>A string formatted in <b>kilobits per second</b> (kb/s).</returns>
    public override string ToString() => $"{KilobitsPerSecond:0.##} kb/s";
}