using EfficientConsoleWriting.Structs;
using Microsoft.Win32.SafeHandles;

namespace EfficientConsoleWriting;

public class ConsoleBufferWriter : IDisposable
{
    private readonly CharInfo[] _buffer;
    private SmallRect _rect;
    private int _pos = 0;
    private readonly SafeFileHandle _handle;

    public ConsoleBufferWriter(SafeFileHandle handle, int width, int height, int left = 0, int top = 0)
    {
        _handle = handle;

        Width = (short)width;
        Height = (short)height;

        Left = (short)left;
        Top = (short)top;

        _buffer = new CharInfo[Width * Height];
        _rect = new SmallRect { Left = Left, Top = Top, Right = (short)(Width + Left), Bottom = (short)(Top + Height) };

        for (int i = 0; i < _buffer.Length; i++)
        {
            _buffer[i].Char.AsciiChar = (byte)' ';
        }

        Foreground = Console.ForegroundColor;
        Background = Console.BackgroundColor;
    }

    public ConsoleColor Foreground { get; set; }
    public ConsoleColor Background { get; set; }

    public short Width { get; }
    public short Height { get; }
    public short Left { get; }
    public short Top { get; }

    public void SetPosition(int x, int y)
    {
        _pos = PositionToIndex(x, y);
    }

    public void WriteAt(int x, int y, char c, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        var pos = PositionToIndex(x, y);

        _buffer[pos].Attributes = MakeAttributes(foreground ?? Foreground, background ?? Background);
        _buffer[pos].Char.AsciiChar = (byte)c;
    }

    private int PositionToIndex(int x, int y)
    {
        var pos = x + y * Width;
        return pos;
    }

    public void Write(char c, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        _buffer[_pos].Attributes = MakeAttributes(foreground ?? Foreground, background ?? Background);
        _buffer[_pos].Char.AsciiChar = (byte)c;

        _pos++;
    }

    public void Write(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        foreach (var c in text)
        {
            Write(c, foreground, background);
        }
    }

    public void WriteLine(string text, ConsoleColor? foreground = null, ConsoleColor? background = null)
    {
        Write(text, foreground, background);
        NewLine();
    }

    private short MakeAttributes(ConsoleColor foreground, ConsoleColor background)
    {
        var offset = ((int)ConsoleColor.White + 1) * (int)background;

        return (short)(foreground + offset);
    }

    public void Dispose()
    {
        var size = new Coord { X = Width, Y = Height };
        var start = new Coord { X = 0, Y = 0 };

        var r = Externs.WriteConsoleOutput(_handle, _buffer, size, start, ref _rect);

        var x = Math.Min(Console.BufferWidth - 1, Left);
        var y = Math.Min(Console.BufferHeight - 1, Top + Height + 1);

        Console.SetCursorPosition(x, y);
    }

    public void NewLine()
    {
        while (_pos % Width != 0)
        {
            _pos++;
        }
    }
}