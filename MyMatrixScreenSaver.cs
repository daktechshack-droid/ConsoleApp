using MyApp.Models;
using MyFirstConsoleApp;
using System.Threading.Tasks;

public static class MyMatrixScreenSaver
{
    public static async Task Start(int screenWidth, int screenHeight)
    {
        MyBuffer myBuffer = new MyBuffer(screenWidth, screenHeight);
        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
        Random rnd = new Random();
        List<MyObject> myObjects = new List<MyObject>();
        var lastY = 0;
        for(int i = 0; i < screenWidth - 1; i++)
        {
            var y = rnd.Next(1, screenHeight - 5) + rnd.Next(1, 5);
            if (lastY == y) y++;
            myObjects.Add(new MyObject(i, y, 5));
            lastY = y;
        }
        Console.CursorVisible = false;
        //MatrixScreenSaver(screenHeight, screenWidth);

        //while (true)
        //{
        //    myBuffer.Clear();
        //    foreach (MyObject obj in myObjects)
        //    {
        //        DoDrawAsync(screenHeight, obj).Wait();
        //    }

        //    if (Console.KeyAvailable)
        //    {
        //        var charPressed = Console.ReadKey(true).Key;
        //        if (charPressed == ConsoleKey.X)
        //        {
        //            break;
        //        }
        //    }
        //}

        while (true)
        {
            var tasks = new List<Task>();
            myBuffer.Clear();
            foreach (MyObject obj in myObjects)
            {
                tasks.Add(DoDrawAsync(screenHeight, obj));
            }

            if (Console.KeyAvailable)
            {
                var charPressed = Console.ReadKey(true).Key;
                if (charPressed == ConsoleKey.X)
                {
                    break;
                }
            }
            await Task.WhenAll(tasks);
        }

        Console.CursorVisible = true;
    }

    public static async Task StartBufferMode(int screenWidth, int screenHeight)
    {
        MyBuffer myBuffer = new MyBuffer(screenWidth, screenHeight);
        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
        Random rnd = new Random();
        List<MyObject> myObjects = new List<MyObject>();
        var lastY = 0;
        for (int i = 0; i < screenWidth - 1; i++)
        {
            var y = rnd.Next(1, screenHeight - 5) + rnd.Next(1, 5);
            if (lastY == y) y++;
            var traillength = rnd.Next(10, screenHeight - 5);
            myObjects.Add(new MyObject(i, y, traillength));
            lastY = y;
        }
        Console.CursorVisible = false;
        //MatrixScreenSaver(screenHeight, screenWidth);
        while (true)
        {
            var tasks = new List<Task>();
            Parallel.ForEach(myObjects, obj =>
            {
                DoDrawBufferAsync(screenHeight, obj, myBuffer).Wait();
            });
            myBuffer.Render();
            if (Console.KeyAvailable)
            {
                var charPressed = Console.ReadKey(true).Key;
                if (charPressed == ConsoleKey.X)
                {
                    break;
                }
            }
            Thread.Sleep(80);
        }
        Console.CursorVisible = true;
    }

    private static async Task DoDrawAsync(int screenHeight, MyObject obj)
    {
        obj.Draw();
        obj.Move(new MyPoint(0, 1)); //obj.Position.Y++;
        if (obj.Position.Y >= screenHeight)
        {
            obj.Position.Y = 0;
        }
    }

    private static async Task DoDrawBufferAsync(int screenHeight, MyObject obj, MyBuffer myBuffer)
    {
        obj.DrawToBuffer(myBuffer);
        obj.Move(new MyPoint(0, 1)); //obj.Position.Y++;
        if (obj.Position.Y >= screenHeight)
        {
            obj.Position.Y = 0;
        }
    }

    static void MatrixScreenSaver(int screenHeight, int screenWidth)
    {
        Random rnd = new Random();
        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
        int i = 0;
        while (i < screenHeight - 1)
        {
            for (int j = 0; j < screenWidth; j++)
            {
                if (j % 2 == 0)
                    Console.SetCursorPosition(j, i);
                else
                    Console.SetCursorPosition(j, screenHeight - 2 - i);
                char c = (char)(rnd.Next(32, 127));
                Console.Write(c);
            }
            i++;
            Thread.Sleep(100);
        }
    }
}