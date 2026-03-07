namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a size in <b>bytes</b>.
/// </summary>
/// <remarks>Valid values are integers greater than or equal to 0.</remarks>
public readonly record struct ByteSize : IComparable<ByteSize>
{
    /// <summary>
    /// Gets the size in <b>bytes</b>.
    /// </summary>
    public long Bytes { get; }

    private ByteSize(long bytes) => Bytes = bytes;

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of bytes.
    /// </summary>
    /// <param name="bytes">The size in bytes.</param>
    /// <returns>A <see cref="ByteSize"/> representing the specified value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="bytes"/> is less than 0.
    /// </exception>
    public static ByteSize Create(long bytes)
    {
        if (bytes < 0)
            throw new ArgumentOutOfRangeException(nameof(bytes), "Byte size cannot be negative.");

        return new ByteSize(bytes);
    }

    /// <summary>
    /// Creates a <see cref="ByteSize"/> from the specified number of bytes,
    /// or returns <see langword="null"/> if the value is invalid.
    /// </summary>
    /// <param name="bytes">The size in bytes.</param>
    /// <returns>
    /// A <see cref="ByteSize"/> representing the specified value, or <see langword="null"/>
    /// if <paramref name="bytes"/> is <see langword="null"/> or less than 0.
    /// </returns>
    public static ByteSize? CreateOrNull(long? bytes)
        => bytes is { } b and >= 0 ? new ByteSize(b) : null;

    /// <summary>
    /// Compares the current value with another <see cref="ByteSize"/>.
    /// </summary>
    /// <param name="other">The value to compare with the current value.</param>
    /// <returns>
    /// A value less than 0 if this instance is less than <paramref name="other"/>,
    /// 0 if they are equal, or a value greater than 0 if this instance is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(ByteSize other) => Bytes.CompareTo(other.Bytes);

    public static bool operator <(ByteSize left, ByteSize right) => left.CompareTo(right) < 0;
    public static bool operator >(ByteSize left, ByteSize right) => left.CompareTo(right) > 0;
    public static bool operator <=(ByteSize left, ByteSize right) => left.CompareTo(right) <= 0;
    public static bool operator >=(ByteSize left, ByteSize right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Returns the string representation of the current size.
    /// </summary>
    /// <returns>A string formatted in <b>bytes</b>.</returns>
    public override string ToString() => $"{Bytes} B";
}