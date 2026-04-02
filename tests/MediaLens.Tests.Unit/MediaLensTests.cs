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
        await Assert.That(() => _sut.InspectAsync(filePath!)!)
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Inspect_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
    {
        // Arrange
        var nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".mp4");

        // Act & Assert
        await Assert.That(() => _sut.InspectAsync(nonExistentFile)!)
            .Throws<FileNotFoundException>();
    }
}
