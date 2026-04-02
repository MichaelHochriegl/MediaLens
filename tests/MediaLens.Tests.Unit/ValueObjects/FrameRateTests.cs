using MediaLens.Models.ValueObjects;

namespace MediaLens.Tests.Unit.ValueObjects;

public class FrameRateTests
{
    [Test]
    public async Task Create_ShouldSetFramesPerSecond_WhenValueIsValid()
    {
        // Arrange
        const double value = 23.976;

        // Act
        var frameRate = FrameRate.Create(value);

        // Assert
        await Assert.That(frameRate.FramesPerSecond).IsEqualTo(value);
    }

    [Test]
    [Arguments(double.NaN)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(0.0)]
    [Arguments(-1.0)]
    public async Task Create_ShouldThrowArgumentOutOfRangeException_WhenValueIsInvalid(double value)
    {
        // Act & Assert
        await Assert.That(() => FrameRate.Create(value))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task CreateOrNull_ShouldReturnFrameRate_WhenValueIsValid()
    {
        // Arrange
        const double value = 23.976;

        // Act
        var frameRate = FrameRate.CreateOrNull(value);

        // Assert
        await Assert.That(frameRate).IsNotNull();
        await Assert.That(frameRate!.Value.FramesPerSecond).IsEqualTo(value);
    }

    [Test]
    [Arguments(null)]
    [Arguments(double.NaN)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(0.0)]
    [Arguments(-1.0)]
    public async Task CreateOrNull_ShouldReturnNull_WhenValueIsInvalid(double? value)
    {
        // Act
        var frameRate = FrameRate.CreateOrNull(value);

        // Assert
        await Assert.That(frameRate).IsNull();
    }

    [Test]
    public async Task ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var frameRate = FrameRate.Create(23.976);

        // Act
        var str = frameRate.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("23.976 fps");
    }

    [Test]
    public async Task Comparison_ShouldWorkCorrectly()
    {
        // Arrange
        var low = FrameRate.Create(23.976);
        var high = FrameRate.Create(60.0);

        // Assert
        await Assert.That(low < high).IsTrue();
        await Assert.That(high > low).IsTrue();
        await Assert.That(low <= high).IsTrue();
        await Assert.That(high >= low).IsTrue();
        await Assert.That(low == FrameRate.Create(23.976)).IsTrue();
        await Assert.That(low.CompareTo(high)).IsLessThan(0);
    }
}
