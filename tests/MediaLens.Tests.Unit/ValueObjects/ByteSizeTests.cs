using MediaLens.Models.ValueObjects;

namespace MediaLens.Tests.Unit.ValueObjects;

public class ByteSizeTests
{
    [Test]
    public async Task Create_ShouldSetBytes_WhenValueIsValid()
    {
        // Arrange
        const long value = 1024L;

        // Act
        var byteSize = ByteSize.Create(value);

        // Assert
        await Assert.That(byteSize.Bytes).IsEqualTo(value);
    }

    [Test]
    public async Task Create_ShouldThrowArgumentOutOfRangeException_WhenValueIsNegative()
    {
        // Act & Assert
        await Assert.That(() => ByteSize.Create(-1L))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task CreateOrNull_ShouldReturnByteSize_WhenValueIsValid()
    {
        // Arrange
        const long value = 1024L;

        // Act
        var byteSize = ByteSize.CreateOrNull(value);

        // Assert
        await Assert.That(byteSize).IsNotNull();
        await Assert.That(byteSize!.Value.Bytes).IsEqualTo(value);
    }

    [Test]
    [Arguments(null)]
    [Arguments(-1L)]
    public async Task CreateOrNull_ShouldReturnNull_WhenValueIsInvalid(long? value)
    {
        // Act
        var byteSize = ByteSize.CreateOrNull(value);

        // Assert
        await Assert.That(byteSize).IsNull();
    }

    [Test]
    public async Task ToString_ShouldReturnFormattedString()
    {
        // Arrange
        var byteSize = ByteSize.Create(1024L);

        // Act
        var str = byteSize.ToString();

        // Assert
        await Assert.That(str).IsEqualTo("1024 B");
    }

    [Test]
    public async Task Comparison_ShouldWorkCorrectly()
    {
        // Arrange
        var low = ByteSize.Create(1024L);
        var high = ByteSize.Create(2048L);

        // Assert
        await Assert.That(low < high).IsTrue();
        await Assert.That(high > low).IsTrue();
        await Assert.That(low <= high).IsTrue();
        await Assert.That(high >= low).IsTrue();
        await Assert.That(low == ByteSize.Create(1024L)).IsTrue();
        await Assert.That(low.CompareTo(high)).IsLessThan(0);
    }
}
