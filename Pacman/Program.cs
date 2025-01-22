using System;
using System.IO;
using System.Threading;

namespace Pacman
{
    internal class Pacman
    {
        static void Main(string[] args)
        {
            const string MapFilePath = "map.txt";

            const char PlayerCell = '@';

            const ConsoleKey ButtonExit = ConsoleKey.Escape;

            bool isRunning = true;

            char[,] map = ReadMap(MapFilePath);

            int pacmanX = 1;
            int pacmanY = 1;
            int score = 0;

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo('w', ConsoleKey.W, false, false, false);

            Console.CursorVisible = false;

            while (isRunning)
            {
                Console.Clear();

                if (Console.KeyAvailable)
                {
                    pressedKey = Console.ReadKey(true);
                }

                HandleInput(pressedKey, ref pacmanX, ref pacmanY, map, ref score);

                Console.ForegroundColor = ConsoleColor.Blue;
                DrawMap(map);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(pacmanX, pacmanY);
                Console.Write(PlayerCell);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(32, 0);
                Console.Write($"Score: {score}");

                Thread.Sleep(250);

                if (pressedKey.Key == ButtonExit)
                {
                    isRunning = false;
                }
            }
        }

        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);

            char[,] map = new char[GetMaxLengthOfLine(file), file.Length];

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    map[x, y] = file[y][x];

            return map;
        }

        private static void DrawMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.Write("\n");
            }
        }

        private static void HandleInput(ConsoleKeyInfo pressedKey, ref int pacmanX, ref int pacmanY, char[,] map, ref int score)
        {
            const char EmptyCell = ' ';
            const char CoinCell = 'o';
            const char WallCell = '#';

            int[] direction = GetDirection(pressedKey);

            int nextPacmanPositionX = pacmanX + direction[0];
            int nextPacmanPositionY = pacmanY + direction[1];

            char nextCell = map[nextPacmanPositionX, nextPacmanPositionY];

            if (nextCell != WallCell)
            {
                pacmanX = nextPacmanPositionX;
                pacmanY = nextPacmanPositionY;

                if (nextCell == CoinCell)
                {
                    score++;
                    map[nextPacmanPositionX, nextPacmanPositionY] = EmptyCell;
                }
            }
        }

        private static int[] GetDirection(ConsoleKeyInfo pressedKey)
        {
            const ConsoleKey CommandMoveUp = ConsoleKey.UpArrow;
            const ConsoleKey CommandMoveDown = ConsoleKey.DownArrow;
            const ConsoleKey CommandMoveLeft = ConsoleKey.LeftArrow;
            const ConsoleKey CommandMoveRight = ConsoleKey.RightArrow;

            int[] direction = { 0, 0 };

            switch (pressedKey.Key)
            {
                case CommandMoveUp:
                    direction[1] -= 1;
                    break;

                case CommandMoveDown:
                    direction[1] += 1;
                    break;

                case CommandMoveLeft:
                    direction[0] -= 1;
                    break;

                case CommandMoveRight:
                    direction[0] += 1;
                    break;
            }

            return direction;
        }

        private static int GetMaxLengthOfLine(string[] lines)
        {
            int maxLength = lines[0].Length;

            foreach (var line in lines)
            {
                if (line.Length > maxLength)
                {
                    maxLength = line.Length;
                }
            }

            return maxLength;
        }
    }
}
