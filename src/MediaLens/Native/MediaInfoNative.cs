using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming due to MediaInfo library naming conventions

namespace MediaLens.Native;

internal static partial class MediaInfoNative
{
    private const string LibraryName = "mediainfo";

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

    internal static MediaInfoHandle New()
        => NewImpl();

    internal static void Delete(IntPtr handle)
        => DeleteImpl(handle);

    internal static nuint Open(MediaInfoHandle handle, string fileName)
        => OperatingSystem.IsWindows()
            ? OpenW(handle, fileName)
            : OpenA(handle, fileName);

    internal static void Close(MediaInfoHandle handle)
        => CloseImpl(handle);

    internal static int CountGet(MediaInfoHandle handle, StreamKind kind, nuint streamNumber)
        => checked((int)CountGetImpl(handle, kind, streamNumber));

    internal static IntPtr Get(
        MediaInfoHandle handle,
        StreamKind kind,
        int streamNumber,
        string parameter,
        InfoKind infoKind,
        InfoKind searchKind)
        => OperatingSystem.IsWindows()
            ? GetW(handle, kind, streamNumber, parameter, infoKind, searchKind)
            : GetA(handle, kind, streamNumber, parameter, infoKind, searchKind);

    internal static IntPtr Option(MediaInfoHandle handle, string option, string? value)
        => OperatingSystem.IsWindows()
            ? OptionW(handle, option, value)
            : OptionA(handle, option, value);

    internal static void OpenBufferInit(MediaInfoHandle handle, ulong fileSize, ulong fileOffset)
        => OpenBufferInitImpl(handle, fileSize, fileOffset);

    internal static bool OpenBufferContinue(MediaInfoHandle handle, byte[] buffer, nuint bufferSize)
        => OpenBufferContinueImpl(handle, buffer, bufferSize) != 0;

    internal static bool OpenBufferFinalize(MediaInfoHandle handle)
        => OpenBufferFinalizeImpl(handle) != 0;

    internal static ulong OpenBufferContinueGoToGet(MediaInfoHandle handle)
        => OpenBufferContinueGoToGetImpl(handle);

    internal static string PtrToString(IntPtr ptr)
    {
        if (ptr == IntPtr.Zero)
            return string.Empty;

        return OperatingSystem.IsWindows()
            ? Marshal.PtrToStringUni(ptr) ?? string.Empty
            : Marshal.PtrToStringUTF8(ptr) ?? string.Empty;
    }

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_New")]
    private static partial MediaInfoHandle NewImpl();

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Delete")]
    private static partial void DeleteImpl(IntPtr handle);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Close")]
    private static partial void CloseImpl(MediaInfoHandle handle);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Count_Get")]
    private static partial nuint CountGetImpl(MediaInfoHandle handle, StreamKind kind, nuint streamNumber);

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

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Open", StringMarshalling = StringMarshalling.Utf16)]
    private static partial nuint OpenW(MediaInfoHandle handle, string fileName);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Get", StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr GetW(
        MediaInfoHandle handle,
        StreamKind kind,
        int streamNumber,
        string parameter,
        InfoKind infoKind,
        InfoKind searchKind);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Option", StringMarshalling = StringMarshalling.Utf16)]
    private static partial IntPtr OptionW(MediaInfoHandle handle, string option, string? value);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Open_Buffer_Init")]
    private static partial void OpenBufferInitImpl(MediaInfoHandle handle, ulong fileSize, ulong fileOffset);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Open_Buffer_Continue")]
    private static partial nuint OpenBufferContinueImpl(MediaInfoHandle handle, byte[] buffer, nuint bufferSize);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Open_Buffer_Finalize")]
    private static partial nuint OpenBufferFinalizeImpl(MediaInfoHandle handle);

    [LibraryImport(LibraryName, EntryPoint = "MediaInfo_Open_Buffer_Continue_GoTo_Get")]
    private static partial ulong OpenBufferContinueGoToGetImpl(MediaInfoHandle handle);
}