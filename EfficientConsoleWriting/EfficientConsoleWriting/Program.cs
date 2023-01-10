using EfficientConsoleWriting;
using Microsoft.Win32.SafeHandles;

var lorem = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

var handle = Externs.CreateConsoleStdOut();

if (handle.IsInvalid)
{
    throw new InvalidOperationException($"{nameof(SafeFileHandle)} is invalid");
}

var foreground = 0;
var background = 4;

using (var cb = new ConsoleBufferWriter(handle, Console.BufferWidth, Console.BufferHeight))
{
    for (int y = 0; y < cb.Height; y++)
    {
        for (int x = 0; x < cb.Width; x++)
        {
            var p = x + (y * cb.Width);

            var c = lorem[p % lorem.Length];

            cb.Write(c, (ConsoleColor)foreground, (ConsoleColor)background);

            foreground++;
            background--;

            if (foreground > (int)ConsoleColor.White)
            {
                foreground = 0;
            }
            if (background < 0)
            {
                background = (int)ConsoleColor.White;
            }
        }
    }
}