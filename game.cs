using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        const int screenHeight = 20;
        const int screenWidth = 40;
        const int groundHeight = 2;

        int playerX = 5;
        int playerY = screenHeight - groundHeight - 1;
        bool isJumping = false;
        int jumpHeight = 5;
        int jumpCounter = 0;

        Random random = new Random();
        int obstacleX = screenWidth - 1;
        int obstacleY = screenHeight - groundHeight - 1;

        int score = 0;
        bool gameOver = false;

        void DrawPlayer()
        {
            Console.SetCursorPosition(playerX, playerY);
            Console.Write("C#");
        }

        void DrawGround()
        {
            for (int x = 0; x < screenWidth; x++)
            {
                Console.SetCursorPosition(x, screenHeight - groundHeight);
                Console.Write("=");
            }
        }

        void DrawObstacle()
        {
            Console.SetCursorPosition(obstacleX, obstacleY);
            Console.Write("|");
        }

        void ClearPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(" ");
        }

        void UpdateObstacle()
        {
            if (obstacleX == playerX && obstacleY == playerY)
            {
                gameOver = true;
            }

            if (obstacleX <= 0)
            {
                obstacleX = screenWidth - 1;
                score++;
            }
            else
            {
                obstacleX--;
            }
        }

        void HandleJump()
        {
            if (isJumping)
            {
                if (jumpCounter < jumpHeight)
                {
                    ClearPosition(playerX, playerY);
                    playerY--;
                    jumpCounter++;
                }
                else
                {
                    isJumping = false;
                }
            }
            else if (playerY < screenHeight - groundHeight - 1)
            {
                ClearPosition(playerX, playerY);
                playerY++;
            }
            else
            {
                jumpCounter = 0;
            }
        }

        while (!gameOver)
        {
            Console.Clear();
            DrawGround();
            DrawPlayer();
            DrawObstacle();

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(intercept: true).Key;
                if (key == ConsoleKey.Spacebar && playerY == screenHeight - groundHeight - 1)
                {
                    isJumping = true;
                }
            }

            UpdateObstacle();
            HandleJump();

            Console.SetCursorPosition(0, 0);
            Console.Write($"Score: {score}");

            Thread.Sleep(150);
        }

        Console.Clear();
        Console.WriteLine("Game Over!");
        Console.WriteLine($"Final Score: {score}");
    }
}
