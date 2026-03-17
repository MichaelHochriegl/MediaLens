using MediaLens.Models;
using MediaLens.Tests.Integration.Extensions;

namespace MediaLens.Tests.Integration;

public class MediaLensIntegrationTests
{
    private const string SampleFileName = "example-video.webm";

    [Test]
    public async Task Inspect_AgainstSampleFile_ReturnsExpectedMediaInfo()
    {
        // Arrange
        var mediaLens = new MediaLens();
        var filePath = Path.Combine(
            TestContext.OutputDirectory ?? AppContext.BaseDirectory,
            SampleFileName);

        // Act
        var info = mediaLens.Inspect(filePath);

        // Assert
        await Assert.That(info).IsNotNull();
        await Verify(info.ToSnapshotModel())
            .ScrubMember<GeneralTrack>(g => g.FileName);
    }

    [Test]
    public async Task TryInspect_AgainstSampleFile_ReturnsSuccessAndExpectedMediaInfo()
    {
        // Arrange
        var mediaLens = new MediaLens();
        var filePath = Path.Combine(
            TestContext.OutputDirectory ?? AppContext.BaseDirectory,
            SampleFileName);

        // Act
        var success = mediaLens.TryInspect(filePath, out var info);

        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(info).IsNotNull();
        
        await Verify(info.ToSnapshotModel())
            .ScrubMember<GeneralTrack>(g => g.FileName);
    }
}
