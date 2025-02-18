using System;
using System.Collections.Generic;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            SnakeGame game = new SnakeGame();
            game.Start();
        }
    }

    class SnakeGame
    {
        private const int Width = 32;
        private const int Height = 16;
        private Snake snake;
        private Pixel berry;
        private Random random;
        private bool isGameOver;
        private int score;

        public SnakeGame()
        {
            Console.WindowHeight = Height;
            Console.WindowWidth = Width;
            random = new Random();
            isGameOver = false;
            score = 5;
            snake = new Snake(Width / 2, Height / 2);
            SpawnBerry();
        }

        public void Start()
        {
            while (!isGameOver)
            {
                DrawGameBoard();
                snake.Move();
                CheckCollision();
                Thread.Sleep(200); // Adjust speed
            }
            EndGame();
        }

        private void DrawGameBoard()
        {
            Console.Clear();
            DrawBorders();
            snake.Draw();
            berry.Draw(ConsoleColor.Cyan);
        }

        private void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < Width; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, Height - 1);
                Console.Write("■");
            }
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(Width - 1, i);
                Console.Write("■");
            }
        }

        private void CheckCollision()
        {
            if (snake.HeadX == 0 || snake.HeadX == Width - 1 ||
                snake.HeadY == 0 || snake.HeadY == Height - 1 ||
                snake.HasSelfCollision())
            {
                isGameOver = true;
            }

            if (snake.HeadX == berry.X && snake.HeadY == berry.Y)
            {
                score++;
                snake.Grow();
                SpawnBerry();
            }
        }

        private void SpawnBerry()
        {
            berry = new Pixel(random.Next(1, Width - 2), random.Next(1, Height - 2));
        }

        private void EndGame()
        {
            Console.SetCursorPosition(Width / 5, Height / 2);
            Console.WriteLine($"Game Over! Score: {score}");
            Console.ReadKey();
        }
    }

    class Snake
    {
        private List<Pixel> body;
        private string direction;

        public int HeadX => body[0].X;
        public int HeadY => body[0].Y;

        public Snake(int startX, int startY)
        {
            body = new List<Pixel> { new Pixel(startX, startY) };
            direction = "RIGHT";
        }

        public void Move()
        {
            ReadInput();
            Pixel newHead = new Pixel(HeadX, HeadY);

            switch (direction)
            {
                case "UP": newHead.Y--; break;
                case "DOWN": newHead.Y++; break;
                case "LEFT": newHead.X--; break;
                case "RIGHT": newHead.X++; break;
            }

            body.Insert(0, newHead);
            body.RemoveAt(body.Count - 1);
        }

        public void Grow()
        {
            body.Add(new Pixel(body[^1].X, body[^1].Y));
        }

        public bool HasSelfCollision()
        {
            for (int i = 1; i < body.Count; i++)
            {
                if (body[i].X == HeadX && body[i].Y == HeadY)
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw()
        {
            foreach (Pixel p in body)
            {
                p.Draw(ConsoleColor.Red);
            }
        }

        private void ReadInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow when direction != "DOWN": direction = "UP"; break;
                    case ConsoleKey.DownArrow when direction != "UP": direction = "DOWN"; break;
                    case ConsoleKey.LeftArrow when direction != "RIGHT": direction = "LEFT"; break;
                    case ConsoleKey.RightArrow when direction != "LEFT": direction = "RIGHT"; break;
                }
            }
        }
    }

    class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Draw(ConsoleColor color)
        {
            Console.SetCursorPosition(X, Y);
            Console.ForegroundColor = color;
            Console.Write("■");
        }
    }
}
