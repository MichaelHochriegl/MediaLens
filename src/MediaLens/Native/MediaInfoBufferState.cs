namespace MediaLens.Native;

[Flags]
internal enum MediaInfoBufferState
{
    MoreDataRequired = 0,
    EnoughDataRead = 1,
}