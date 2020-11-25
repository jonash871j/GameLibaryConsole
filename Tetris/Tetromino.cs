using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisLogic
{
    public class Tetromino
    {
        private Field[] array;

        public int Rotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Field FieldType { get; private set; }
        public Field this[int x, int y]
        {
            get
            {
                if (CheckInbound(x, y))
                {
                    switch(Rotation)
                    {
                    case 90: return array[12 + y - (x * 4)];
                    case 180: return array[15 - (y * 4) - x];
                    case 270: return array[3 - y + (x * 4)];
                    default: return array[y * Width + x];
                    }
                }
                else
                    return Field.Empty;
            }
        }

        public Tetromino(string pattern, Field fieldType, int width = 4, int height = 4)
        {
            // Sets properties
            Width = width;
            Height = height;
            FieldType = fieldType;
            array = new Field[Width * Height];

            // Check if mismatch between tetromino data and size
            if (pattern.Length != Width * Height)
                throw new Exception("Mismatch between tetromino data and size!");

            // Adds data to tetromino array
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (pattern[y * Width + x] == 'X')
                        array[y * Width + x] = fieldType;
                    else
                        array[y * Width + x] = Field.Empty;
                }
            }
        }

        public bool CheckInbound(int x, int y)
        {
            if ((x < 0) || (y < 0) || (x >= Width) || (y >= Height))
                return false;
            else
                return true;
        }

        public void Reset(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Rotate(Direction direction)
        {
            Rotation += (int)direction * 90;

            if (Rotation < 0)   Rotation = 270;
            if (Rotation > 270) Rotation = 0;
        }
        public void Transform(Direction x, Direction y)
        {
            X += (int)x;
            Y += (int)y;
        }

        public Tetromino Clone()
        {
            return (Tetromino)this.MemberwiseClone();
        }
    }
}
