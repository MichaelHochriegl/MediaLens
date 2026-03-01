namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a bitrate in <b>bits per second</b> (b/s).
/// </summary>
public readonly record struct BitRate : IComparable<BitRate>
{
    /// <summary>Gets the bitrate value in <b>bits per second</b> (b/s).</summary>
    public double BitsPerSecond { get; }

    private BitRate(double bitsPerSecond) => BitsPerSecond = bitsPerSecond;

    /// <summary>
    /// Creates a <see cref="BitRate"/> from a finite bits-per-second value (must be &gt;= 0).
    /// </summary>
    public static BitRate Create(double bitsPerSecond)
    {
        if (double.IsNaN(bitsPerSecond) || double.IsInfinity(bitsPerSecond) || bitsPerSecond < 0)
            throw new ArgumentOutOfRangeException(nameof(bitsPerSecond), "Bitrate must be a finite number greater than or equal to 0.");

        return new BitRate(bitsPerSecond);
    }

    /// <summary>
    /// Creates a <see cref="BitRate"/> if the input is valid; otherwise returns <see langword="null"/>.
    /// </summary>
    public static BitRate? CreateOrNull(double? bitsPerSecond)
        => bitsPerSecond is { } bps &&
           !double.IsNaN(bps) &&
           !double.IsInfinity(bps) &&
           bps >= 0
            ? new BitRate(bps)
            : null;

    /// <summary>Gets the bitrate in kilobits per second (kb/s), SI units (1 kb/s = 1000 b/s).</summary>
    public double KilobitsPerSecond => BitsPerSecond / 1000.0;

    public int CompareTo(BitRate other) => BitsPerSecond.CompareTo(other.BitsPerSecond);

    public static bool operator <(BitRate left, BitRate right) => left.CompareTo(right) < 0;
    public static bool operator >(BitRate left, BitRate right) => left.CompareTo(right) > 0;
    public static bool operator <=(BitRate left, BitRate right) => left.CompareTo(right) <= 0;
    public static bool operator >=(BitRate left, BitRate right) => left.CompareTo(right) >= 0;

    public override string ToString() => $"{KilobitsPerSecond:0.##} kb/s";
}