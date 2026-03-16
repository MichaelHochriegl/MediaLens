# MediaLens

MediaLens is a managed .NET wrapper around the native [MediaInfo](https://mediaarea.net/en/MediaInfo) library.

It provides a simple API for inspecting media files and extracting metadata such as:

- container format
- duration
- file size
- overall bitrate
- video stream details
- audio stream details
- text/subtitle stream details

## Installation

```bash
dotnet add package MediaLens
```

If you want to register it through dependency injection:

```bash
dotnet add package MediaLens.DependencyInjection
```

## Quick start

```csharp
using MediaLens;

var mediaLens = new MediaLens.MediaLens();

var mediaInfo = mediaLens.Inspect("movie.mkv");

Console.WriteLine($"File: {mediaInfo.General.FileName}");
Console.WriteLine($"Format: {mediaInfo.General.Format}");
Console.WriteLine($"Duration: {mediaInfo.General.Duration}");

foreach (var video in mediaInfo.VideoTracks)
{
    Console.WriteLine($"Video: {video.Format} {video.Width}x{video.Height} {video.FrameRate}");
}

foreach (var audio in mediaInfo.AudioTracks)
{
    Console.WriteLine($"Audio: {audio.Format} {audio.Channels}ch {audio.Language}");
}

foreach (var text in mediaInfo.TextTracks)
{
    Console.WriteLine($"Subtitle: {text.Format} {text.Language}");
}
```

## Dependency injection

If your application uses `Microsoft.Extensions.DependencyInjection`, you can register `IMediaLens` like this:

```csharp
using MediaLens.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediaLens();
```
This will register `IMediaLens` as a singleton.

You can also choose a specific lifetime:

```csharp
using MediaLens.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediaLens(ServiceLifetime.Scoped);
```

Then consume it through `IMediaLens`:

```csharp
using MediaLens;

public sealed class MediaInspector(IMediaLens mediaLens)
{
    public void Print(string path)
    {
        var info = mediaLens.Inspect(path);
        Console.WriteLine(info.General.Format);
    }
}
```

## API overview

The main abstraction is `IMediaLens`:

```csharp
MediaInfo Inspect(string filePath);
bool TryInspect(string filePath, out MediaInfo? info);
```

### Inspect

`Inspect` returns a `MediaInfo` instance for a file and throws if the file cannot be inspected.

Use this when failure should be explicit.

### TryInspect

`TryInspect` returns `false` instead of throwing for expected inspection failures.

Use this when you prefer a simpler success/failure flow.

## Returned metadata

`MediaInfo` contains:

- `General`
- `VideoTracks`
- `AudioTracks`
- `TextTracks`

A typical inspection result gives you access to values such as:

### General

- file name
- format
- duration
- file size
- overall bitrate mode
- overall bitrate

### Video tracks

- format
- codec id
- language
- frame rate
- frame rate mode
- width
- height
- bitrate
- aspect ratio

### Audio tracks

- format
- codec id
- language
- channel count
- channel layout
- sampling rate
- bitrate

### Text tracks

- format
- language

## Error handling

MediaLens exposes a small set of exceptions for common failure cases:

- `MediaLensException` (all of the exceptions further below extend this)
- `MediaLensOpenException`
- `MediaLensHandleException`
- `MediaLensNativeDependencyException`


## Example

A minimal example is included in the repository under `examples/`.

## License & attribution
- **This repository and packaging code**: MIT (`LICENSE`).
- **MediaInfoLib (bundled in the native binaries of the referenced nuget MediaLens.Native)**: BSD-2-Clause (`LICENSE.MediaInfo`), included in the package.
