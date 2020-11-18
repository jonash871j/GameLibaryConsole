using System;
using System.IO;
using System.Text;

namespace Engine
{
    public class Sprite
    {
        char[] asciiArray = new char[]
        {
            '\0', '☺', '☻', '♥', '♦', '♣', '♠', '•', '◘', '○', '◙', '♂', '♀', '♪', '♫', '☼', '►',
            '◄', '↕', '‼', '¶', '§', '▬', '↨', '↑', '↓', '→', '←', '∟', '↔', '▲', '▼', ' ', '!',
            '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2',
            '3', '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?', '@', 'A', 'B', 'C',
            'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', '^', '_', '`', 'a', 'b', 'c', 'd', 'e',
            'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
            'w', 'x', 'y', 'z', '{', '|', '}', '~', '⌂', 'Ç', 'ü', 'é', 'â', 'ä', 'à', 'å', 'ç',
            'ê', 'ë', 'è', 'ï', 'î', 'ì', 'Ä', 'Å','É', 'æ', 'Æ', 'ô', 'ö', 'ò', 'û', 'ù', 'ÿ',
            'Ö', 'Ü', 'ø', '£', 'Ø', '×', 'ƒ', 'á','í', 'ó', 'ú', 'ñ', 'Ñ', 'ª', 'º', '¿', '®',
            '¬', '½', '¼', '¡', '«', '»', '░', '▒','▓', '│', '┤', 'Á', 'Â', 'À', '©', '╣', '║',
            '╗', '╝', '¢', '¥', '┐', '└', '┴', '┬','├', '─', '┼', 'ã', 'Ã', '╚', '╔', '╩', '╦',
            '╠', '═', '╬', '¤', 'ð', 'Ð', 'Ê', 'Ë','È', 'ı', 'Í', 'Î', 'Ï', '┘', '┌', '█', '▄',
            '¦', 'Ì', '▀', 'Ó', 'ß', 'Ô', 'Ò', 'õ','Õ', 'µ', 'þ', 'Þ', 'Ú', 'Û', 'Ù', 'ý', 'Ý',
            '¯', '´', '-', '±', '‗', '¾', '¶', '§', '÷', '¸', '°', '¨', '·', '¹', '³', '²', '■',
        };

        public string Path { get; private set; } = "";
        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public char[,] CharMap { get; private set; } = null;
        public byte[,] ColorMap { get; private set; } = null;

        public Sprite() { }
        public Sprite(string path)
        {
            Import(path);
        }

        public void Import(string path)
        {
            Path = path;


            string extension = System.IO.Path.GetExtension(path);

            switch (extension)
            {
                case ".ascspr": ImportASCSPR(path); break;
                case ".acxspr": ImportACXSPR(path); break;
                case ".acspr" : ImportACSPR(path);  break;
                default:        throw new Exception("Invalid character sprite file extension: " + path);
            }

        }

        private void ImportASCSPR(string path)
        {
            FileHandler inFile = new FileHandler(path, false);
            int mode = inFile.ReadBYTE();
            Width = inFile.ReadDWORD();
            Height = inFile.ReadDWORD();

            CharMap = new char[Height, Width];
            ColorMap = new byte[Height, Width];

            void FileMode0()
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        CharMap[y, x] = asciiArray[inFile.ReadBYTE()];
                        ColorMap[y, x] = (byte)inFile.ReadBYTE();
                    }
                }
            }

            switch (mode)
            {
                case 0: FileMode0(); break;
                default: FileMode0(); break;
            }

            inFile.Close();
        }
        private void ImportACXSPR(string path)
        {
            FileHandler inFile = new FileHandler(path, false);

            inFile.Skip(3);
            Width = inFile.ReadInt();
            Height = inFile.ReadInt();

            CharMap = new char[Height, Width];
            ColorMap = new byte[Height, Width];

            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (i == 0)
                            CharMap[y, x] = asciiArray[inFile.ReadBYTE()];
                        else
                            ColorMap[y, x] = (byte)inFile.ReadBYTE();
                    }
                }
            }
        }
        private void ImportACSPR(string path)
        {
            FileHandler inFile = new FileHandler(path, false);
            
            Width = inFile.ReadInt();
            inFile.Skip(1);
            Height = inFile.ReadInt();
            inFile.Skip(1);

            CharMap = new char[Height, Width];
            ColorMap = new byte[Height, Width];

            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (i == 0)
                            CharMap[y, x] = asciiArray[inFile.ReadBYTE()];
                        else
                            ColorMap[y, x] = (byte)inFile.ReadBYTE();
                    }
                    inFile.Skip(2);
                }
                inFile.Skip(2);
            }
        }
    }
}
