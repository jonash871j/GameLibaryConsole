using System;
using System.Collections.Generic;
using System.Text;

namespace TetrisLogic
{
    public class Container
    {
        private Tetromino tetromino = null;
        public Tetromino Tetromino 
        {
            get
            {
                if (tetromino != null)
                    return new Tetromino(tetromino.Pattern, tetromino.FieldType, tetromino.ResetRotation, tetromino.Width, tetromino.Height);
                else
                    return new Tetromino(".........", Field.Cyan);
            } 
        }
        public bool IsEmpty { get; private set; } = true;

        /// <summary>
        /// Clears container
        /// </summary>
        public void Clear()
        {
            tetromino = null;
            IsEmpty = true;
        }

        /// <summary>
        /// Swaps cotainer out with tetromino
        /// </summary>
        /// <returns>cotainers tetromino</returns>
        public Tetromino Swap(Tetromino tetromino)
        {
            if (this.tetromino == null)
            {
                IsEmpty = false;
                this.tetromino = tetromino.Clone();
                return Tetromino;
            }
            else
            {
                Tetromino oldTetromino = this.tetromino;
                this.tetromino = tetromino;
                return oldTetromino;
            }
        }
    }
}
