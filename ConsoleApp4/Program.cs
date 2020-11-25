﻿using System;
using Engine;
using Games;

class Program
{
    static void UpdateWindow()
    {
        ConsoleEx.Create(64, 32);
        ConsoleEx.SetFont("Terminal", 16, 16);
    }

    static void Main(string[] args)
    {
        Game[] games = new Game[]
        {
            new TicTacToe(),
            new RockPaperScissors(),
            new Tetris(),
        };

        UpdateWindow();

        while (true)
        {
            ConsoleEx.WriteLine("- Game libary\n");

            for (int i = 0; i < games.Length; i++)
            {
                ConsoleEx.WriteLine("PRESS '" + (char)('A' + i) + "' TO PLAY " + games[i].Name);

                if (Input.KeyPressed((Key)('A' + i)))
                {
                    games[i].Play();
                    UpdateWindow();
                }
            }
            ConsoleEx.Update();
            ConsoleEx.Clear();
        }
    }
}