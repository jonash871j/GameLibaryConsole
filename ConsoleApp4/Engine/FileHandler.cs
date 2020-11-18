using System.Collections.Generic;

namespace Engine
{
    internal class FileHandler
    {
        internal System.IO.FileStream File { get; private set; }
        private bool isBigEndian;

        internal FileHandler(string path, bool isBigEndian)
        {
            File = System.IO.File.Open(path, System.IO.FileMode.Open);
            this.isBigEndian = isBigEndian;
        }

        internal long ReadQWORD()
        {
            if (!isBigEndian)
                return (ReadBYTE() << 0) | (ReadBYTE() << 8) | (ReadBYTE() << 16) | (ReadBYTE() << 24) | (ReadBYTE() << 32) | (ReadBYTE() << 40) | (ReadBYTE() << 48) | (ReadBYTE() << 56);
            else
                return (ReadBYTE() << 56) | (ReadBYTE() << 48) | (ReadBYTE() << 40) | (ReadBYTE() << 32) | (ReadBYTE() << 24) | (ReadBYTE() << 16) | (ReadBYTE() << 8) | (ReadBYTE() << 0);
        }
        internal int ReadDWORD()
        {
            if (!isBigEndian)
                return (ReadBYTE() << 0) | (ReadBYTE() << 8) | (ReadBYTE() << 16) | (ReadBYTE() << 24);
            else
                return (ReadBYTE() << 24) | (ReadBYTE() << 16) | (ReadBYTE() << 8) | (ReadBYTE() << 0);
        }
        internal int ReadTBYTE()
        {
            if (!isBigEndian)
                return (ReadBYTE() << 0) | (ReadBYTE() << 8) | (ReadBYTE() << 16);
            else
                return (ReadBYTE() << 16) | (ReadBYTE() << 8) | (ReadBYTE() << 0);
        }
        internal short ReadWORD()
        {
            if (!isBigEndian)
                return (short)((ReadBYTE() << 0) | (ReadBYTE() << 8));
            else
                return (short)((ReadBYTE() << 8) | (ReadBYTE() << 0));
        }
        internal byte ReadBYTE()
        {
            try
            {
                return (byte)File.ReadByte();
            }
            catch
            {
                return 0;
            }
        }
        internal int ReadInt()
        {
            List<char> numberCharacters = new List<char>();

            bool CheckNumber(byte val)
            {
                for (int i = 48; i < 58; i++)
                    if (val == i)
                        return true;
                return false;
            }

            while(true)
            {
                byte numberCharacter = ReadBYTE();

                if (CheckNumber(numberCharacter))
                    numberCharacters.Add((char)numberCharacter);
                else break;
            }

            try
            {
                return int.Parse(new string(numberCharacters.ToArray()));
            }
            catch
            {
                return 0;
            }
        }
        internal void Skip(int amount)
        {
            for (int i = 0; i < amount; i++)
                ReadBYTE();
        }
        internal void Close()
        {
            File.Close();
        }
    }
}
