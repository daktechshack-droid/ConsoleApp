using MyApp.Models;
using MyFirstConsoleApp;
using System.Threading.Tasks;

public static class MyMatrixScreenSaver
{
    public static async Task Start(int screenWidth, int screenHeight)
    {
        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
        Random rnd = new Random();
        List<MyObject> myObjects = new List<MyObject>();
        //MyObject myObject = new MyObject(0, 0, 10);
        for(int i = 0; i < screenWidth - 1; i++)
        {
            myObjects.Add(new MyObject(i, rnd.Next(0, screenHeight - 2), 5));
        }

        //MatrixScreenSaver(screenHeight, screenWidth);
        while (true)
        {
            var tasks = new List<Task>();
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

    }

    private static async Task DoDrawAsync(int screenHeight, MyObject obj)
    {
        obj.Draw();
        obj.Position.Y++;
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