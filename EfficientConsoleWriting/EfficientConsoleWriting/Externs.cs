using System.Runtime.InteropServices;
using EfficientConsoleWriting.Structs;
using Microsoft.Win32.SafeHandles;

namespace EfficientConsoleWriting;

public static class Externs
{
    public static SafeFileHandle CreateConsoleStdOut()
    {
        var h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

        if (h.IsInvalid)
        {
            throw new InvalidOperationException("Unable to create stdout file handle");
        }

        return h;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteConsoleOutput(
        SafeFileHandle hConsoleOutput,
        CharInfo[] lpBuffer,
        Coord dwBufferSize,
        Coord dwBufferCoord,
        ref SmallRect lpWriteRegion);

    [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern SafeFileHandle CreateFile(
        string fileName,
        [MarshalAs(UnmanagedType.U4)] uint fileAccess,
        [MarshalAs(UnmanagedType.U4)] uint fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] int flags,
        IntPtr template);
}