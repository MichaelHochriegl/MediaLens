using MediaLens.Exceptions;

namespace MediaLens.Tests.Unit;

public class MediaLensTests
{
    private readonly MediaLens _sut = new();

    [Test]
    [Arguments(null!)]
    [Arguments("")]
    [Arguments("  ")]
    public async Task Inspect_ShouldThrowArgumentException_WhenFilePathIsNullOrWhiteSpace(string? filePath)
    {
        // Act & Assert
        await Assert.That(() => _sut.Inspect(filePath!))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Inspect_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");

        // Act & Assert
        await Assert.That(() => _sut.Inspect(nonExistentFile))
            .Throws<FileNotFoundException>();
    }

    [Test]
    [Arguments(null!)]
    [Arguments("")]
    [Arguments("  ")]
    public async Task TryInspect_ShouldThrowArgumentException_WhenFilePathIsNullOrWhiteSpace(string? filePath)
    {
        // Act & Assert
        await Assert.That(() => _sut.TryInspect(filePath!, out _))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task TryInspect_ShouldReturnFalse_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");

        // Act
        var result = _sut.TryInspect(nonExistentFile, out var info);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(info).IsNull();
    }
}
