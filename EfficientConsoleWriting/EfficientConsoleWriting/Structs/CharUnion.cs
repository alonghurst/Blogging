using System.Runtime.InteropServices;

namespace EfficientConsoleWriting.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct CharUnion
{
    [FieldOffset(0)] public ushort UnicodeChar;
    [FieldOffset(0)] public byte AsciiChar;
}
