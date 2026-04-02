# MediaLens

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/MediaLens)](https://www.nuget.org/packages/MediaLens/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![CI](https://github.com/MichaelHochriegl/MediaLens/actions/workflows/ci.yml/badge.svg)](https://github.com/MichaelHochriegl/MediaLens/actions/workflows/ci.yml)

MediaLens is a small .NET wrapper around the native [MediaInfo](https://mediaarea.net/en/MediaInfo) library.
It comes with the bundled native binaries for (by using [MediaLens.Native](https://github.com/MichaelHochriegl/MediaLens.Native)):
- Windows x64
- Linux x64
- macOS x64
- macOS arm64

## Features

- async `InspectAsync` API
- strongly typed metadata models
- dependency injection support
- native MediaInfo integration

## Installation

```bash
dotnet add package MediaLens
```

If you want DI support:

```bash
dotnet add package MediaLens.DependencyInjection
```

## Quick start

```csharp
using MediaLens;

var mediaLens = new MediaLens.MediaLens();
var mediaInfo = await mediaLens.InspectAsync("media.mp4");

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

```csharp
using MediaLens.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediaLens();
```

`AddMediaLens()` registers `IMediaLens` as a singleton by default.

You can also choose a different lifetime:

```csharp
using MediaLens.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediaLens(ServiceLifetime.Scoped);
```

## Returned metadata

`InspectAsync()` returns a `MediaInfo` object with:

- general file metadata
- video track metadata
- audio track metadata
- text/subtitle track metadata

## Error handling

Common exceptions include:

- `MediaLensException`
- `MediaLensOpenException`
- `MediaLensHandleException`
- `MediaLensNativeDependencyException`

## Example

A minimal example is available under `examples/`.

## License

- **Repository code**: MIT (`LICENSE`)
- **MediaInfoLib**: BSD-2-Clause (`LICENSE.MediaInfo`)

MediaInfo is used through the bundled native binaries from `MediaLens.Native`.