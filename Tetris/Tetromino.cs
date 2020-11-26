using System;

namespace TetrisLogic
{
    public class Tetromino
    {
        private Field[] array;

        public int Rotation { get; set; } = 270;
        public int ResetRotation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int Size { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Pattern { get; private set; }
        public Field FieldType { get; private set; }
        public Field this[int x, int y]
        {
            get
            {
                return GetField(Rotation, x, y);
            }
        }

        public Tetromino(string pattern, Field fieldType, int resetRotation = 0, int width = 3, int height = 3)
        {
            // Sets properties
            ResetRotation = resetRotation;
            Rotation = resetRotation;
            Width = width;
            Height = height;
            Size = Width * Height; 
            FieldType = fieldType;
            Pattern = pattern;
            array = new Field[Size];

            // Check if mismatch between tetromino data and size
            if (pattern.Length != Size)
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

        /// <summary>
        /// Used to check if coordinate is inbound of tetromino
        /// </summary>
        /// <returns>false when out of bound</returns>
        private bool CheckInbound(int x, int y)
        {
            if ((x < 0) || (y < 0) || (x >= Width) || (y >= Height))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Used to reset tetromino
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Reset(int x, int y)
        {
            X = x;
            Y = y;
            Rotation = ResetRotation;
        }

        /// <summary>
        /// Used to rotate tetromino
        /// </summary>
        public void Rotate(Direction direction)
        {
            Rotation += (int)direction * 90;

            if (Rotation < 0)   Rotation = 270;
            if (Rotation > 270) Rotation = 0;
        }

        /// <summary>
        /// Used to move tetromino
        /// </summary>
        public void Transform(Direction x, Direction y)
        {
            X += (int)x;
            Y += (int)y;
        }

        /// <summary>
        /// Used to clone current tetromino
        /// </summary>
        public Tetromino Clone()
        {
            return (Tetromino)this.MemberwiseClone();
        }

        public Field GetField(int rotation, int x, int y)
        {
            if (CheckInbound(x, y))
            {
                switch (rotation)
                {
                case 90 : return array[(Size - Height) + y - (x * Width)];
                case 180: return array[(Size - 1) - (y * Height) - x];
                case 270: return array[(Width - 1) - y + (x * Width)];
                default : return array[y * Width + x];
                }
            }
            return Field.Empty;
        }
    }
}
