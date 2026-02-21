namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a size in <b>bytes</b>.
/// </summary>
/// <remarks>Valid values are integers greater than or equal to 0.</remarks>
public readonly record struct ByteSize : IComparable<ByteSize>
{
    /// <summary>Gets the size in <b>bytes</b>.</summary>
    public long Bytes { get; }

    private ByteSize(long bytes) => Bytes = bytes;

    public static ByteSize Create(long bytes)
    {
        if (bytes < 0)
            throw new ArgumentOutOfRangeException(nameof(bytes), "Byte size cannot be negative.");

        return new ByteSize(bytes);
    }

    public static ByteSize? CreateOrNull(long? bytes)
        => bytes is { } b and >= 0 ? new ByteSize(b) : null;

    public int CompareTo(ByteSize other) => Bytes.CompareTo(other.Bytes);

    public static bool operator <(ByteSize left, ByteSize right) => left.CompareTo(right) < 0;
    public static bool operator >(ByteSize left, ByteSize right) => left.CompareTo(right) > 0;
    public static bool operator <=(ByteSize left, ByteSize right) => left.CompareTo(right) <= 0;
    public static bool operator >=(ByteSize left, ByteSize right) => left.CompareTo(right) >= 0;

    public override string ToString() => $"{Bytes} B";
}