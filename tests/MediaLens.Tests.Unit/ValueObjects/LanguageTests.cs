using MediaLens.Models.ValueObjects;

namespace MediaLens.Tests.Unit.ValueObjects;

public class LanguageTests
{
    [Test]
    public async Task Constructor_ShouldSetValue_WhenValueIsValid()
    {
        // Arrange
        const string value = "en";

        // Act
        var language = new Language(value);

        // Assert
        await Assert.That(language.Value).IsEqualTo(value);
    }

    [Test]
    [Arguments(null!)]
    [Arguments("")]
    [Arguments("  ")]
    public async Task Constructor_ShouldThrowArgumentException_WhenValueIsInvalid(string? value)
    {
        // Act & Assert
        await Assert.That(() => new Language(value!))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task CreateOrNull_ShouldReturnLanguage_WhenValueIsValid()
    {
        // Arrange
        const string value = "en";

        // Act
        var language = Language.CreateOrNull(value);

        // Assert
        await Assert.That(language).IsNotNull();
        await Assert.That(language!.Value).IsEqualTo(value);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("  ")]
    public async Task CreateOrNull_ShouldReturnNull_WhenValueIsInvalid(string? value)
    {
        // Act
        var language = Language.CreateOrNull(value);

        // Assert
        await Assert.That(language).IsNull();
    }

    [Test]
    public async Task ToString_ShouldReturnUnderlyingValue()
    {
        // Arrange
        const string value = "en";
        var language = new Language(value);

        // Act
        var str = language.ToString();

        // Assert
        await Assert.That(str).IsEqualTo(value);
    }

    [Test]
    public async Task Equality_ShouldWorkCorrectly()
    {
        // Arrange
        var lang1 = new Language("en");
        var lang2 = new Language("en");
        var lang3 = new Language("fr");

        // Assert
        await Assert.That(lang1).IsEqualTo(lang2);
        await Assert.That(lang1).IsNotEqualTo(lang3);
    }
}
