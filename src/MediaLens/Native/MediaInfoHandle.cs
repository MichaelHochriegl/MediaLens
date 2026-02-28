using Microsoft.Win32.SafeHandles;

namespace MediaLens.Native;

internal sealed class MediaInfoHandle() : SafeHandleZeroOrMinusOneIsInvalid(ownsHandle: true)
{
    // internal static MediaInfoHandle Create()
    // {
    //     var ptr = MediaInfoNative.New();
    //     if (ptr == IntPtr.Zero)
    //         throw new InvalidOperationException("Failed to create MediaInfo handle.");
    //
    //     var h = new MediaInfoHandle();
    //     h.SetHandle(ptr);
    //     return h;
    // }

    protected override bool ReleaseHandle()
    {
        MediaInfoNative.Delete(handle);
        return true;
    }
}