var mediaLens = new MediaLens.MediaLens();

var file = Path.Combine(Directory.GetCurrentDirectory(), "example-video.webm");
Console.WriteLine($"Analyzing file: {file}");
var mediaInfo = mediaLens.Analyze(file);

Console.WriteLine(mediaInfo);