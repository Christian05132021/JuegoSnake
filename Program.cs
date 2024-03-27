using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        enum Direction { Up, Down, Left, Right }

        static int width = 40;
        static int height = 20;
        static int score = 0;
        static int delay = 100;
        static bool gameOver = false;
        static Random random = new Random();

        static List<int[]> snake = new List<int[]>();
        static int[] food = new int[2];

        static Direction currentDirection = Direction.Right;

        static void Main(string[] args)
        {
            SetupGame();
            DrawGame();

            Thread inputThread = new Thread(ReadInput);
            inputThread.Start();

            while (!gameOver)
            {
                MoveSnake();
                if (IsEatingFood())
                {
                    score++;
                    DrawFood();
                }
                Thread.Sleep(delay);
            }

            EndGame();
        }

        static void SetupGame()
        {
            Console.Title = "Juego de la serpiente";
            Console.CursorVisible = false;
            Console.SetWindowSize(width + 1, height + 2);
            Console.SetBufferSize(width + 1, height + 2);

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Cyan; //cambiar los colores
            Console.Clear();

            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.Write("Presiona para empezar");

            Console.ReadKey();
            Console.Clear();

            DrawBorder();
            DrawSnake();
            DrawFood();
        }

        static void DrawBorder()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red; 

            // Draw horizontal borders
            for (int i = 0; i < width + 2; i++)
            {
                Console.SetCursorPosition(i, 1);
                Console.Write("=");
                Console.SetCursorPosition(i, height + 1);
                Console.Write("=");
            }

            // Draw vertical borders
            for (int i = 1; i < height + 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
                Console.SetCursorPosition(width + 1, i);
                Console.Write("|");
            }
        }

        static void DrawSnake()
        {
            snake.Clear();
            snake.Add(new int[] { width / 2, height / 2 });
            Console.SetCursorPosition(snake[0][0], snake[0][1]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("<>");
        }

        static void DrawFood()
        {
            food[0] = random.Next(1, width);
            food[1] = random.Next(1, height);

            Console.SetCursorPosition(food[0], food[1]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("0");
        }

        static void DrawGame()
        {
            DrawBorder();
            DrawSnake();
            DrawFood();
        }

        static void MoveSnake()
        {
            int[] newHead = { snake[0][0], snake[0][1] };

            switch (currentDirection)
            {
                case Direction.Up:
                    newHead[1]--;
                    break;
                case Direction.Down:
                    newHead[1]++;
                    break;
                case Direction.Left:
                    newHead[0]--;
                    break;
                case Direction.Right:
                    newHead[0]++;
                    break;
            }

            if (newHead[0] <= 0 || newHead[0] >= width + 1 || newHead[1] <= 0 || newHead[1] >= height + 1)
            {
                gameOver = true;
                return;
            }

            Console.SetCursorPosition(snake[snake.Count - 1][0], snake[snake.Count - 1][1]);
            Console.Write(" ");
            snake.RemoveAt(snake.Count - 1);

            if (snake.Contains(newHead))
            {
                gameOver = true;
                return;
            }

            snake.Insert(0, newHead);
            Console.SetCursorPosition(newHead[0], newHead[1]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("#");
        }

        static bool IsEatingFood()
        {
            if (snake[0][0] == food[0] && snake[0][1] == food[1])
            {
                snake.Add(new int[] { food[0], food[1] });
                return true;
            }
            return false;
        }

        static void ReadInput()
        {
            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            if (currentDirection != Direction.Down)
                                currentDirection = Direction.Up;
                            break;
                        case ConsoleKey.DownArrow:
                            if (currentDirection != Direction.Up)
                                currentDirection = Direction.Down;
                            break;
                        case ConsoleKey.LeftArrow:
                            if (currentDirection != Direction.Right)
                                currentDirection = Direction.Left;
                            break;
                        case ConsoleKey.RightArrow:
                            if (currentDirection != Direction.Left)
                                currentDirection = Direction.Right;
                            break;
                        default:
                            gameOver = true;
                            break;
                    }
                }
            }
        }

        static void EndGame()
        {
            Console.SetCursorPosition(width / 2 - 5, height / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Se acabo el FUCKING JUEGO!");
            Console.SetCursorPosition(width / 2 - 8, height / 2 + 1);
            Console.Write($"Tu record: {score}");
            Console.SetCursorPosition(0, height + 1);
        }
    }
}