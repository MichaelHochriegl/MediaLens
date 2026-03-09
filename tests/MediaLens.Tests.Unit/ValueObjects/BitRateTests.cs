using MediaLens.Models.ValueObjects;

namespace MediaLens.Tests.Unit.ValueObjects;

public class BitRateTests
{
    [Test]
    public async Task Create_ShouldSetBitsPerSecond_WhenValueIsValid()
    {
        // Arrange
        const double value = 1000.0;

        // Act
        var bitRate = BitRate.Create(value);

        // Assert
        await Assert.That(bitRate.BitsPerSecond).IsEqualTo(value);
    }

    [Test]
    [Arguments(double.NaN)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(-1.0)]
    public async Task Create_ShouldThrowArgumentOutOfRangeException_WhenValueIsInvalid(double value)
    {
        // Act & Assert
        await Assert.That(() => BitRate.Create(value))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task CreateOrNull_ShouldReturnBitRate_WhenValueIsValid()
    {
        // Arrange
        const double value = 1000.0;

        // Act
        var bitRate = BitRate.CreateOrNull(value);

        // Assert
        await Assert.That(bitRate).IsNotNull();
        await Assert.That(bitRate!.Value.BitsPerSecond).IsEqualTo(value);
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
        var bitRate = BitRate.CreateOrNull(value);

        // Assert
        await Assert.That(bitRate).IsNull();
    }

    [Test]
    public async Task KilobitsPerSecond_ShouldReturnCorrectValue()
    {
        // Arrange
        var bitRate = BitRate.Create(1500.0);

        // Act
        var kbps = bitRate.KilobitsPerSecond;

        // Assert
        await Assert.That(kbps).IsEqualTo(1.5);
    }

    [Test]
    public async Task ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var bitRate = BitRate.Create(1500.0);

        // Act
        var str = bitRate.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("1.5 kb/s");
    }

    [Test]
    public async Task Comparison_ShouldWorkCorrectly()
    {
        // Arrange
        var low = BitRate.Create(1000.0);
        var high = BitRate.Create(2000.0);

        // Assert
        await Assert.That(low < high).IsTrue();
        await Assert.That(high > low).IsTrue();
        await Assert.That(low <= high).IsTrue();
        await Assert.That(high >= low).IsTrue();
        await Assert.That(low == BitRate.Create(1000.0)).IsTrue();
        await Assert.That(low.CompareTo(high)).IsLessThan(0);
    }
}
