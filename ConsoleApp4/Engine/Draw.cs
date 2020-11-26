using Engine;

namespace Engine
{
	public static class Draw
	{
		/// <summary>
		/// Used to draw sprite
		/// </summary>
		public static void Sprite(int x1, int y1, Sprite sprite)
        {
			if ((x1 + sprite.Width < 0) || (y1 + sprite.Height < 0) ||
				(x1 + sprite.Width >= ConsoleEx.Width + sprite.Width) || (y1 + sprite.Height >= ConsoleEx.Height + sprite.Height))
				return;

			for (int y = 0; y < sprite.Height; y++)
            {
				int ySum = y1 + y;

				for (int x = 0; x < sprite.Width; x++)
                {
					if (sprite.CharMap[y, x] != '\0')
						ConsoleEx.WriteCharacter(x1 + x, ySum, sprite.CharMap[y, x], sprite.ColorMap[y, x]);
                }
            }
        }

		/// <summary>
		/// Used to draw tile map
		/// </summary>
		public static void TileMap(int x1, int y1, TileMap tileMap)
        {
			for (int y = 0; y < tileMap.Height; y++)
			{
				for (int x = 0; x < tileMap.Width; x++)
				{
					Sprite sprite = tileMap.Sprites[(int)tileMap.IdMap[y, x]];

					if (sprite != null)
						Sprite(x1 + (x * tileMap.TileWidth), y1 + (y * tileMap.TileHeight), sprite);
				}
			}
		}

		/// <summary>
		/// Used to draw background
		/// </summary>
		public static void Background(char character = '█', byte color = Color.Silver)
		{
			for (int y = 0; y < ConsoleEx.Height; y++)
			{
				for (int x = 0; x < ConsoleEx.Width; x++)
				{
					ConsoleEx.WriteCharacter(x, y, character, color);
				}
			}
		}

		/// <summary>
		/// Used to draw sprite background
		/// </summary>
		public static void Background(Sprite sprite, int xOffset = 0, int yOffset = 0)
        {
			for (int y = -ConsoleEx.Height + (yOffset % sprite.Height); y < ConsoleEx.Height + sprite.Height; y += sprite.Height)
            {
				for (int x = -ConsoleEx.Width +(xOffset % sprite.Width); x < ConsoleEx.Width + sprite.Width; x += sprite.Width)
                {
					Sprite(x, y, sprite);
                }
            }
        }

		/// <summary>
		/// Used to draw a line
		/// </summary>
		public static void Line(int x1, int y1, int x2, int y2, char character = '█', byte color = Color.Silver)
        {
			// Calculation of Slope
			float ax = ((float)y2 - (float)y1) / ((float)x2 - (float)x1);
			float ay = ((float)x2 - (float)x1) / ((float)y2 - (float)y1);

			// Cordinate system
			int xp = x2 - x1, xm = -(x2 - x1);
			int yp = y2 - y1, ym = -(y2 - y1);
			int x, y;

			// Draw start- and end point
			ConsoleEx.WriteCharacter(x1, y1, character, color);
			ConsoleEx.WriteCharacter(x2, y2, character, color);

			// All drawing and calculation
			// SLOPE AX																	
			if ((xp >= ym) && (yp <= xp))												
				for (x = 0; x < xp; x++)
					ConsoleEx.WriteCharacter(x1 + x, (int)(y1 + ax * x), character, color);			
			else																		
				// SLOPE -AY															
				if (ym >= xm)															
					for (y = ym; y > 0; y--)
						ConsoleEx.WriteCharacter((int)(x1 -(ay * y)), y1 -y, character, color);		
				else																	
																				
					// SLOPE -AX	 													
					if (yp <= xm)														
						for (x = xm; x > 0; x--)
					ConsoleEx.WriteCharacter(x1 -x, (int)(y1 -(ax * x)), character, color);	
																				
					// SLOPE AY															
					else																
						for (y = 0; y < yp; y++)
							ConsoleEx.WriteCharacter((int)(x1 + ay * y), y1 + y, character, color);	
        }

		/// <summary>
		/// Used to draw rectangle
		/// </summary>
		public static void Rectangle(int x1, int y1, int x2, int y2, bool outline = false, char character = '█', byte color = Color.Silver)
		{
			// Swap values if the first value is negative
			if (x2 < x1)
			{
				int x2Store = x2;
				x2 = x1;
				x1 = x2Store;
			}
			if (y2 < y1)
			{
				int y2Store = y2;
				y2 = x1;
				y1 = y2Store;
			}

			// Draw rectangle
			for (int y = y1; y <= y2; y++)
			{
				for (int x = x1; x <= x2; x++)
				{
					// Rectangle fill
					if (outline == false)
						ConsoleEx.WriteCharacter(x, y, character, color);
					// Rectangle Outline
					else if ((x == x1) || (y == y1) || (x == x2) || (y == y2))
						ConsoleEx.WriteCharacter(x, y, character, color);
				}
			}
		}
	}
}