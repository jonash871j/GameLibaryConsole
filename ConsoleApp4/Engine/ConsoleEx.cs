using System;
using System.Data;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading;

namespace Engine
{
    public enum TextDirection
    {
        LeftToRight = 0,
        RightToLeft = 1,
        TopToButtom = 2,
        ButtomToTop = 3,
    }

    public static class Color
    {
        public const byte Black = 0;
        public const byte Blue = 1;
        public const byte Green = 2;
        public const byte Teal = 3;
        public const byte Red = 4;
        public const byte Purple = 5;
        public const byte Brown = 6;
        public const byte Silver = 7;
        public const byte Grey = 8;
        public const byte Sky = 9;
        public const byte Lime = 10;
        public const byte Cyan = 11;
        public const byte Salmon = 12;
        public const byte Magenta = 13;
        public const byte Yellow = 14;
        public const byte White = 15;

        public static byte Get(byte foreground, byte background)
        {
            return (byte)(foreground + 16 * background);
        }
    }

    public class ConsoleEx
    {
        #region LowLevel
        [StructLayout(LayoutKind.Sequential)]
        private struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;

            public SmallRect(short left, short top, short right, short bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct CharUnion
        {
            [FieldOffset(0)] public ushort UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
        private struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public ushort Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ConsoleScreenBufferInfoEX
        {
            public uint cbSize;
            public Coord dwSize;
            public Coord dwCursorPosition;
            public short wAttributes;
            public SmallRect srWindow;
            public Coord dwMaximumWindowSize;
            public short wPopupAttributes;
            public int bFullscreenSupported;
            public uint Color00;
            public uint Color01;
            public uint Color02;
            public uint Color03;
            public uint Color04;
            public uint Color05;
            public uint Color06;
            public uint Color07;
            public uint Color08;
            public uint Color09;
            public uint Color10;
            public uint Color11;
            public uint Color12;
            public uint Color13;
            public uint Color14;
            public uint Color15;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ConsoleFontInfoEX
        {
            public uint cbSize;
            public uint nFont;
            public Coord dwFontSize;
            public uint fontFamily;
            public uint FontWeight;
            public ushort faceName00;
            public ushort faceName01;
            public ushort faceName02;
            public ushort faceName03;
            public ushort faceName04;
            public ushort faceName05;
            public ushort faceName06;
            public ushort faceName07;
            public ushort faceName08;
            public ushort faceName09;
            public ushort faceName10;
            public ushort faceName11;
            public ushort faceName12;
            public ushort faceName13;
            public ushort faceName14;
            public ushort faceName15;
            public ushort faceName16;
            public ushort faceName17;
            public ushort faceName18;
            public ushort faceName19;
            public ushort faceName20;
            public ushort faceName21;
            public ushort faceName22;
            public ushort faceName23;
            public ushort faceName24;
            public ushort faceName25;
            public ushort faceName26;
            public ushort faceName27;
            public ushort faceName28;
            public ushort faceName29;
            public ushort faceName30;
            public ushort faceName31;
        }

        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateConsoleScreenBuffer(
             UInt32 dwDesiredAccess,
             UInt32 dwShareMode,
             IntPtr secutiryAttributes,
             UInt32 flags,
             IntPtr screenBufferData
            );
        [DllImport("Kernel32.dll")]
        private static extern IntPtr SetConsoleActiveScreenBuffer(IntPtr handle);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "WriteConsoleOutputW")]
        private static extern bool WriteConsoleOutput(
               IntPtr hConsoleOutput,
               CharInfo[,] lpBuffer,
               Coord dwBufferSize,
               Coord dwBufferCoord,
               ref SmallRect lpWriteRegion
               );

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref ConsoleScreenBufferInfoEX consoleScreenBufferInfoEX);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref ConsoleScreenBufferInfoEX consoleScreenBufferInfoEX);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(uint nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwModes);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleHandle, Coord dwSize);

        [DllImport("kernel32.dll")]
        private static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool bMaximumWindow, ref ConsoleFontInfoEX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        private static Coord cStart;
        private static Coord cEnd;
        private static SmallRect windowRect;
        private static IntPtr hConsoleOutput;
        private static IntPtr hConsoleInput;
        private static CharInfo[,] buffer;
        private static ConsoleScreenBufferInfoEX csbiInfo;
        private static ConsoleFontInfoEX cfi;
        #endregion

        public static short Width { get; private set; }
        public static short Height { get; private set; }

        public static int CursorX { get; set; } = 0;
        public static int LineX { get; set; } = 0;
        public static int LineY { get; set; } = 0;

        private static void SetConsoleSize()
        {
            csbiInfo.wAttributes = 0;
            csbiInfo.srWindow = new SmallRect(0, 0, Width, Height);
            csbiInfo.dwSize = new Coord(Width, Height);
        }

        /// <summary>
        /// Creates console buffer
        /// </summary>
        public static void Create(short width, short height)
        {
            Width = width;
            Height = height;

            // Set console input handle
            hConsoleInput = GetStdHandle(0xFFFFFFF6); // 0xFFFFFFF6 = STD_INPUT_HANDLE
            hConsoleOutput = GetStdHandle(0xFFFFFFF5); // 0xFFFFFFF5 = STD_OUTPUT_HANDLE

            SetConsoleMode(hConsoleInput, 136 | 0x0008 | 0x0010 | 0x0001); // ENABLE_EXTENDED_FLAGS | ENABLE_WINDOW_INPUT | ENABLE_MOUSE_INPUT | ENABLE_PROCESSED_INPUT

            cStart = new Coord(0, 0);
            cEnd = new Coord(Width, Height);
            windowRect = new SmallRect(0, 0, Width, Height);

            buffer = new CharInfo[Height, Width];

            SetConsoleActiveScreenBuffer(hConsoleOutput);

            SetFont();
            SetColorPalette(new ColorPalette());
            Clear();
        }

        /// <summary>
        /// Used to set font
        /// </summary>
        public static void SetFont(string fontName = "Consolas", int xSize = 10, int ySize = 20)
        {
            cfi.cbSize = 84;
            cfi.nFont = 0;
            cfi.dwFontSize.X = (short)xSize;
            cfi.dwFontSize.Y = (short)ySize;
            cfi.fontFamily = 0 << 4;
            cfi.FontWeight = 400;
            
            ushort[] wcharArray = new ushort[32];

            for (int i = 0; i < 32; i++)
            {
                if (i < fontName.Length)
                    wcharArray[i] = (ushort)fontName[i];
                else if (i == fontName.Length)
                    wcharArray[i] = 0x0000;
                else
                    wcharArray[i] = 0xfefe;

            }

            cfi.faceName00 = wcharArray[0];
            cfi.faceName01 = wcharArray[1];
            cfi.faceName02 = wcharArray[2];
            cfi.faceName03 = wcharArray[3];
            cfi.faceName04 = wcharArray[4];
            cfi.faceName05 = wcharArray[5];
            cfi.faceName06 = wcharArray[6];
            cfi.faceName07 = wcharArray[7];
            cfi.faceName08 = wcharArray[8];
            cfi.faceName09 = wcharArray[9];
            cfi.faceName10 = wcharArray[10];
            cfi.faceName11 = wcharArray[11];
            cfi.faceName12 = wcharArray[12];
            cfi.faceName13 = wcharArray[13];
            cfi.faceName14 = wcharArray[14];
            cfi.faceName15 = wcharArray[15];
            cfi.faceName16 = wcharArray[16];
            cfi.faceName17 = wcharArray[17];
            cfi.faceName18 = wcharArray[18];
            cfi.faceName19 = wcharArray[19];
            cfi.faceName20 = wcharArray[20];
            cfi.faceName21 = wcharArray[21];
            cfi.faceName22 = wcharArray[22];
            cfi.faceName23 = wcharArray[23];
            cfi.faceName24 = wcharArray[24];
            cfi.faceName25 = wcharArray[25];
            cfi.faceName26 = wcharArray[26];
            cfi.faceName27 = wcharArray[27];
            cfi.faceName28 = wcharArray[28];
            cfi.faceName29 = wcharArray[29];
            cfi.faceName30 = wcharArray[30];
            cfi.faceName31 = wcharArray[31];

            SetCurrentConsoleFontEx(hConsoleOutput, false, ref cfi);
        }

        /// <summary>
        /// Used to set or change console color palette
        /// </summary>
        public static void SetColorPalette(ColorPalette colorPalette)
        {
            csbiInfo.cbSize = 96;
            
            GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbiInfo);
            SetConsoleSize();

            // Fixed array sizes is only allowed in unsafe mode :/
            csbiInfo.Color00 = colorPalette.Colors[0];
            csbiInfo.Color01 = colorPalette.Colors[1];
            csbiInfo.Color02 = colorPalette.Colors[2];
            csbiInfo.Color03 = colorPalette.Colors[3];
            csbiInfo.Color04 = colorPalette.Colors[4];
            csbiInfo.Color05 = colorPalette.Colors[5];
            csbiInfo.Color06 = colorPalette.Colors[6];
            csbiInfo.Color07 = colorPalette.Colors[7];
            csbiInfo.Color08 = colorPalette.Colors[8];
            csbiInfo.Color09 = colorPalette.Colors[9];
            csbiInfo.Color10 = colorPalette.Colors[10];
            csbiInfo.Color11 = colorPalette.Colors[11];
            csbiInfo.Color12 = colorPalette.Colors[12];
            csbiInfo.Color13 = colorPalette.Colors[13];
            csbiInfo.Color14 = colorPalette.Colors[14];
            csbiInfo.Color15 = colorPalette.Colors[15];

            SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbiInfo);
        }

        /// <summary>
        /// Writes buffer to console
        /// </summary>
        public static void Update()
        {
            LineX = 0;
            CursorX = 0;
            LineY = 0;

            WriteConsoleOutput(hConsoleOutput, buffer, cEnd, cStart, ref windowRect);

            GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbiInfo);
            SetConsoleSize();
            SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbiInfo);

            Input.Update();
        }

        /// <summary>
        /// Used to get character directly from console buffer
        /// </summary>
        /// <returns>character</returns>
        public static char GetChar(int x, int y)
        {
            if ((y < Height) && (y >= 0) && (x < Width) && (x >= 0))
            {
                return (char)buffer[y, x].Char.UnicodeChar;
            }
            return '\0';
        }

        /// <summary>
        /// Used to get color directly from console buffer
        /// </summary>
        /// <returns>color</returns>
        public static byte GetColor(int x, int y)
        {
            if ((y < Height) && (y >= 0) && (x < Width) && (x >= 0))
            {
                return (byte)buffer[y, x].Attributes;
            }
            return 0;
        }

        /// <summary>
        /// Used to clear console
        /// </summary>
        public static void Clear()
        {
            Draw.Background(' ', 0);
        }

        /// <summary>
        /// Writes char to console at a specific coordinate 
        /// </summary>
        public static void WriteCharacter(int x, int y, ushort character = '█', byte color = Color.Silver)
        {
            if ((y < Height) && (y >= 0) && (x < Width) && (x >= 0))
            {
                buffer[y, x].Attributes = color;
                buffer[y, x].Char.UnicodeChar = character;
            }
        }

        /// <summary>
        /// Writes string to console at a specific coordinate 
        /// </summary>
        public static void WriteCoord(int x, int y, string text = "", byte color = Color.Silver, TextDirection direction = TextDirection.LeftToRight)
        {
            LineX = x;
            LineY = y;

            // Gets character array from text
            char[] textArray = text.ToCharArray();
            int offset = 0;

            // Draw text to screen
            for (int i = 0; i < text.Length; i++)
            {
                // Checks for linebreak
                if (textArray[i] == '\n')
                {
                    if (direction == TextDirection.LeftToRight) y++;
                    if (direction == TextDirection.RightToLeft) y--;
                    if (direction == TextDirection.TopToButtom) x++;
                    if (direction == TextDirection.ButtomToTop) x--;

                    WriteLine();
                    offset = 0;
                    continue;
                }

                // Switch text drawing direction
                switch (direction)
                {
                case TextDirection.LeftToRight: WriteCharacter(x + offset, y, textArray[i], color); break;
                case TextDirection.RightToLeft: WriteCharacter(x - offset, y, textArray[i], color); break;
                case TextDirection.TopToButtom: WriteCharacter(x, y + offset, textArray[i], color); break;
                case TextDirection.ButtomToTop: WriteCharacter(x, y - offset, textArray[i], color); break;
                }

                if (textArray[i] != '\n')
                    offset++;

                CursorX++;
            }
        } 

        public static void Write(string text, byte color = Color.Silver)
        {
            // Gets character array from text
            char[] textArray = text.ToCharArray();
            int offset = 0;

            int x = LineX + CursorX;

            // Draw text to screen
            for (int i = 0; i < text.Length; i++)
            {
                // Checks for linebreak
                if (textArray[i] == '\n')
                {
                    WriteLine();
                    offset = 0;
                    continue;
                }

                WriteCharacter(x + offset, LineY, textArray[i], color);

                if (textArray[i] != '\n')
                    offset++;

                CursorX++;
            }
        }
        public static void WriteLine(string text, byte color = Color.Silver)
        {
            Write(text, color);
            WriteLine();
        }
        public static void WriteLine()
        {
            CursorX = 0;
            LineY++;
        }

        public static void SetPosition(int x, int y)
        {
            LineX = x;
            LineY = y;
        }
    }
}