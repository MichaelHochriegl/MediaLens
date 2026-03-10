using MediaLens.Models.ValueObjects;

namespace MediaLens.Tests.Unit.ValueObjects;

public class FrequencyTests
{
    [Test]
    public async Task Create_ShouldSetHertz_WhenValueIsValid()
    {
        // Arrange
        const double value = 44100.0;

        // Act
        var frequency = Frequency.Create(value);

        // Assert
        await Assert.That(frequency.Hertz).IsEqualTo(value);
    }

    [Test]
    [Arguments(double.NaN)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(-1.0)]
    public async Task Create_ShouldThrowArgumentOutOfRangeException_WhenValueIsInvalid(double value)
    {
        // Act & Assert
        await Assert.That(() => Frequency.Create(value))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task CreateOrNull_ShouldReturnFrequency_WhenValueIsValid()
    {
        // Arrange
        const double value = 44100.0;

        // Act
        var frequency = Frequency.CreateOrNull(value);

        // Assert
        await Assert.That(frequency).IsNotNull();
        await Assert.That(frequency!.Value.Hertz).IsEqualTo(value);
    }

    [Test]
    [Arguments(null)]
    [Arguments(double.NaN)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(-1.0)]
    public async Task CreateOrNull_ShouldReturnNull_WhenValueIsInvalid(double? value)
    {
        // Act
        var frequency = Frequency.CreateOrNull(value);

        // Assert
        await Assert.That(frequency).IsNull();
    }

    [Test]
    public async Task Kilohertz_ShouldReturnCorrectValue()
    {
        // Arrange
        var frequency = Frequency.Create(48000.0);

        // Act
        var khz = frequency.Kilohertz;

        // Assert
        await Assert.That(khz).IsEqualTo(48.0);
    }

    [Test]
    public async Task ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var frequency = Frequency.Create(48000.0);

        // Act
        var str = frequency.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("48 kHz");
    }

    [Test]
    public async Task Comparison_ShouldWorkCorrectly()
    {
        // Arrange
        var low = Frequency.Create(44100.0);
        var high = Frequency.Create(48000.0);

        // Assert
        await Assert.That(low < high).IsTrue();
        await Assert.That(high > low).IsTrue();
        await Assert.That(low <= high).IsTrue();
        await Assert.That(high >= low).IsTrue();
        await Assert.That(low == Frequency.Create(44100.0)).IsTrue();
        await Assert.That(low.CompareTo(high)).IsLessThan(0);
    }
}
