using MediaLens.Tests.Integration.Extensions;

namespace MediaLens.Tests.Integration;

public class MediaLensIntegrationTests
{
    [Test]
    [Arguments("example-video.webm")]
    [Arguments("example-video-Æ.webm")]
    public async Task Inspect_AgainstSampleFile_ReturnsExpectedMediaInfo(string fileName)
    {
        // Arrange
        var mediaLens = new MediaLens();
        var filePath = Path.Combine(
            TestContext.OutputDirectory ?? AppContext.BaseDirectory,
            fileName);

        // Act
        var info = mediaLens.Inspect(filePath);

        // Assert
        await Assert.That(info).IsNotNull();
        await Verify(info.ToSnapshotModel());
    }

    [Test]
    [Arguments("example-video.webm")]
    [Arguments("example-video-Æ.webm")]
    public async Task TryInspect_AgainstSampleFile_ReturnsSuccessAndExpectedMediaInfo(string fileName)
    {
        // Arrange
        var mediaLens = new MediaLens();
        var filePath = Path.Combine(
            TestContext.OutputDirectory ?? AppContext.BaseDirectory,
            fileName);

        // Act
        var success = mediaLens.TryInspect(filePath, out var info);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(info).IsNotNull();

        await Verify(info.ToSnapshotModel());
    }
}
