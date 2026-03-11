var mediaLens = new MediaLens.MediaLens();

var file = Path.Combine(Directory.GetCurrentDirectory(), "example-video.webm");
Console.WriteLine($"Analyzing file: {file}");
var mediaInfo = mediaLens.Inspect(file);

Console.WriteLine(mediaInfo);

foreach (var track in mediaInfo.VideoTracks)
{
    Console.WriteLine(track);
}

foreach (var track in mediaInfo.AudioTracks)
{
    Console.WriteLine(track);
}