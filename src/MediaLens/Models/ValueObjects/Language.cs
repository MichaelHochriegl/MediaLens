namespace MediaLens.Models.ValueObjects;

/// <summary>
/// Represents a language identifier reported by media metadata.
/// </summary>
/// <remarks>
/// The value is preserved as provided and must be a non-empty, non-whitespace string.
/// </remarks>
public sealed record Language
{
    /// <summary>
    /// Gets the language identifier value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Language"/> type.
    /// </summary>
    /// <param name="value">The language identifier value.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/>, empty, or consists only of whitespace.
    /// </exception>
    public Language(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }

    /// <summary>
    /// Creates a <see cref="Language"/> from the specified value,
    /// or returns <see langword="null"/> if the value is invalid.
    /// </summary>
    /// <param name="value">The language identifier value.</param>
    /// <returns>
    /// A <see cref="Language"/> representing the specified value, or <see langword="null"/>
    /// if <paramref name="value"/> is <see langword="null"/>, empty, or consists only of whitespace.
    /// </returns>
    public static Language? CreateOrNull(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : new Language(value);

    /// <summary>
    /// Returns the string representation of the current language identifier.
    /// </summary>
    /// <returns>The underlying language identifier value.</returns>
    public override string ToString() => Value;
}