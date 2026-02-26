using MyApp.Models;
using MyFirstConsoleApp;

public static class PongGame
{
    public static void Start(int screenWidth = 80, int screenHeight = 25)
    {
        var myObject = new MyObject(10, 5);
        var ballPos = myObject.Position;
        var direction = new MyPoint(1, 1);
        var lastBallPos = new MyPoint(ballPos);
        var batPos = new MyPoint(screenWidth / 2, screenHeight - 2);
        var batLast = new MyPoint(batPos);
        var score = 0;
        var batLength = 12;
        var batSpeed = 8;
        float speed  = 1f;

        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
        Console.CursorVisible = false;
        MyUIHelper.WriteCentered("Pong Game - Press X to exit", screenHeight - 1);
        while (true)
        {
            Console.SetCursorPosition((int)lastBallPos.X, (int)lastBallPos.Y);
            Console.Write(" ");

            Thread.Sleep(50);
            if (ballPos.X > screenWidth - 2)
            {
                direction.X = -speed;
                Task.Run(() => Console.Beep(440, 500));
            }
            //if (start.Y > screenHeight - 2) direction.Y = -speed;

            if (ballPos.X < 1)
            {
                direction.X = speed;
                Task.Run(() => Console.Beep(440, 500));
            }
            if (ballPos.Y < 1)
            {
                direction.Y = speed;
                Task.Run(() => Console.Beep(440, 500));
            }
            
            lastBallPos = new MyPoint(ballPos);

            ballPos.X += direction.X;
            ballPos.Y += direction.Y;
            if (ballPos.X >= batPos.X && ballPos.X <= batPos.X + batLength && ballPos.Y == batPos.Y)
            {
                direction.Y = -speed;
                ballPos.Y += direction.Y;
                score += 10;
                myObject.AddTrailPoint(new MyObjectChar(ballPos, '.'));
                Task.Run(() => Console.Beep(540, 500));
            }

            MyUIHelper.WriteCentered($"SCORE: {score}", (int)(0));

            myObject.Draw();
            //Console.SetCursorPosition((int)ballPos.X, (int)ballPos.Y);
            //Console.Write("O");
            if (ballPos.Y == screenHeight - 2)
            {
                Task.Run(() => Console.Beep(240, 800));
                Console.SetCursorPosition((int)lastBallPos.X, (int)lastBallPos.Y);
                Console.Write(" ");
                MyUIHelper.WriteCentered("GAME OVER!!!", (int)(ballPos.Y / 2));
                break;
            }

            if (Console.KeyAvailable)
            {
                var charPressed = Console.ReadKey(true).Key;
                if (charPressed == ConsoleKey.D || charPressed == ConsoleKey.RightArrow)
                {
                    batLast = new MyPoint(batPos);
                    if (batPos.X < screenWidth - 2)
                    {
                        batPos.X += batSpeed;
                    }
                }
                else if (charPressed == ConsoleKey.A || charPressed == ConsoleKey.LeftArrow)
                {
                    batLast = new MyPoint(batPos);
                    if(batPos.X > 0)
                    {
                        batPos.X += -batSpeed;
                    }                    
                }
                else if (charPressed == ConsoleKey.X)
                {
                    break;
                }
            }
            DrawBat(batPos, batLast, batLength, batSpeed);

        }
        Console.CursorVisible = true;
        MyUIHelper.WriteCentered("Pong Game Ended - Press any key to exit", (int)(ballPos.Y + 1));
        Console.ReadKey(true);

        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
    }

    public static void DrawBat(MyPoint p, MyPoint last, int length, int batSpeed)
    {
        var bat = string.Empty;
        if (p.X != last.X)
        {
            Console.SetCursorPosition((int)last.X, (int)last.Y);
            bat = new String(' ', length);
            Console.Write(bat);
        }

        if(p.X < 0) p.X = 0;
        if(p.X > Console.WindowWidth - length) p.X = Console.WindowWidth - length;
        Console.SetCursorPosition((int)p.X, (int)p.Y);
        bat = new String('^', length);
        Console.Write(bat);
    }
}