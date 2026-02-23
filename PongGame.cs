using MyApp.Models;
using MyFirstConsoleApp;

public static class PongGame
{
    public static void Start(int screenWidth = 80, int screenHeight = 25)
    {
        var start = new MyPoint(10, 5);
        var direction = new MyPoint(1, 1);
        var last = new MyPoint(start);
        var batPos = new MyPoint(0, screenHeight - 2);
        var batLast = new MyPoint(batPos);
        var batLength = 8;
        var batSpeed = 8;
        float speed  = 1f;

        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
        Console.CursorVisible = false;
        MyUIHelper.WriteCentered("Pong Game - Press X to exit", screenHeight - 1);
        while (true)
        {
            Console.SetCursorPosition((int)last.X, (int)last.Y);
            Console.Write(" ");

            Thread.Sleep(50); 
            if (start.X > screenWidth - 2) direction.X = -speed;
            //if (start.Y > screenHeight - 2) direction.Y = -speed;
            
            if (start.X < 1) direction.X = speed;
            if (start.Y < 1) direction.Y = speed;
            
            last = new MyPoint(start);

            if (batPos.X >= start.X && batPos.X <= start.X + batLength && start.Y == batPos.Y)
            {
                direction.Y = -speed;
                // do nothing
            }
            if (start.Y == screenHeight - 2)
            {
                MyUIHelper.WriteAt("GAME OVER!!!", (int)(start.X + 1), (int)(start.Y));
                break;
            }
            start.X += direction.X;
            start.Y += direction.Y;

            Console.SetCursorPosition((int)start.X, (int)start.Y);
            Console.Write("O");
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
        MyUIHelper.WriteAt("Pong Game Ended - Press any key to exit", (int)(start.X + 1), (int)(start.Y + 1));
        Console.ReadKey(true);
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

        Console.SetCursorPosition((int)p.X, (int)p.Y);
        bat = new String('_', length);
        Console.Write(bat);
    }
}