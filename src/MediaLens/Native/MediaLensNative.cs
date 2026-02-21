using System;
using System.Runtime.InteropServices;

namespace MediaLens.Native;

internal static partial class MediaInfoNative
{
#if WINDOWS
    private const string LibraryName = "MediaInfo.dll";
#elif OSX
    private const string LibraryName = "libmediainfo.dylib";
#else
    private const string LibraryName = "libmediainfo.so";
#endif

    internal enum StreamKind
    {
        General = 0,
        Video = 1,
        Audio = 2,
        Text = 3,
        Other = 4,
        Image = 5,
        Menu = 6
    }

    internal enum InfoKind
    {
        Name = 0,
        Text = 1,
        Measure = 2,
        Options = 3,
        Name_Text = 4,
        Measure_Text = 5,
        Info = 6,
        HowTo = 7
    }

    internal static IntPtr New()
        => NewImpl();

    internal static void Delete(IntPtr handle)
        => DeleteImpl(handle);

    internal static nuint Open(MediaInfoHandle handle, string fileName)
#if WINDOWS
        => OpenW(handle, fileName);
#else
        => OpenA(handle, fileName);
#endif

    internal static void Close(MediaInfoHandle handle)
        => CloseImpl(handle);

    internal static int CountGet(MediaInfoHandle handle, StreamKind kind, int streamNumber)
        => CountGetImpl(handle, kind, streamNumber);

    internal static IntPtr Get(
        MediaInfoHandle handle,
        StreamKind kind,
        int streamNumber,
        string parameter,
        InfoKind infoKind,
        InfoKind searchKind)
#if WINDOWS
        => GetW(handle, kind, streamNumber, parameter, infoKind, searchKind);
#else
        => GetA(handle, kind, streamNumber, parameter, infoKind, searchKind);
#endif

    internal static IntPtr Option(MediaInfoHandle handle, string option, string? value)
#if WINDOWS
        => OptionW(handle, option, value);
#else
        => OptionA(handle, option, value);
#endif

    internal static string PtrToString(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return string.Empty;

#if WINDOWS
        return Marshal.PtrToStringUni(ptr) ?? string.Empty;
#else
        return Marshal.PtrToStringUTF8(ptr) ?? string.Empty;
#endif
    }

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_New")]
    private static partial IntPtr NewImpl();

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Delete")]
    private static partial void DeleteImpl(IntPtr handle);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Close")]
    private static partial void CloseImpl(MediaInfoHandle handle);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Count_Get")]
    private static partial int CountGetImpl(MediaInfoHandle handle, StreamKind kind, int streamNumber);

#if !WINDOWS
    [LibraryImport(LibraryName, EntryPoint = "MediaInfoA_Open", StringMarshalling = StringMarshalling.Utf8)]
    private static partial nuint OpenA(MediaInfoHandle handle, string fileName);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfoA_Get", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr GetA(
        MediaInfoHandle handle,
        StreamKind kind,
        int streamNumber,
        string parameter,
        InfoKind infoKind,
        InfoKind searchKind);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfoA_Option", StringMarshalling = StringMarshalling.Utf8)]
    private static partial IntPtr OptionA(MediaInfoHandle handle, string option, string? value);
#endif

#if WINDOWS
    [LibraryImport(LibraryName, EntryPoint = "MediaInfoW_Open", StringMarshalling = StringMarshalling.Utf16)]
    private static partial nuint OpenW(MediaInfoHandle handle, string fileName);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfoW_Get", StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr GetW(
        MediaInfoHandle handle,
        StreamKind kind,
        int streamNumber,
        string parameter,
        InfoKind infoKind,
        InfoKind searchKind);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfoW_Option", StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr OptionW(MediaInfoHandle handle, string option, string? value);
#endif
}
