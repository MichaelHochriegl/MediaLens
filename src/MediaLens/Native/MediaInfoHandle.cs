using Microsoft.Win32.SafeHandles;

namespace MediaLens.Native;

internal sealed class MediaInfoHandle() : SafeHandleZeroOrMinusOneIsInvalid(ownsHandle: true)
{
    protected override bool ReleaseHandle()
    {
        MediaInfoNative.Delete(handle);
        return true;
    }
}