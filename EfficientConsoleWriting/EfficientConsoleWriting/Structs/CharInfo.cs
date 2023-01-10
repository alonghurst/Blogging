using System.Runtime.InteropServices;

namespace EfficientConsoleWriting.Structs;

[StructLayout(LayoutKind.Explicit)]
public struct CharInfo
{
    [FieldOffset(0)] public CharUnion Char;
    [FieldOffset(2)] public short Attributes;
}
