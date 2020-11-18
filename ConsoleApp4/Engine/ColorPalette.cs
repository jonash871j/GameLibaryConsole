using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine
{
    public class ColorPalette
    {
        public string Path { get; private set; } = "";
        public uint[] Colors { get; private set; } = new uint[16] 
        { 
            0x000000, 0xAA0000, 0x00AA00, 0xAAAA00, 0x0000AA, 0xAA00AA, 0x0055AA, 0xAAAAAA,
            0x555555, 0xFF5555, 0x55FF55, 0xFFFF55, 0x5555FF, 0xFF55FF, 0x55FFFF, 0xFFFFFF,
        };

        public ColorPalette() { }
        public ColorPalette(string path)
        {
            Import(path);
        }

        public void Import(string path)
        {
            Path = path;
            FileHandler inFile = new FileHandler(path, false);
            inFile.Skip(4);
            for (int i = 0; i < 16; i++)
                Colors[i] = (uint)inFile.ReadTBYTE();
                
            inFile.Close();
        }
    }
}
