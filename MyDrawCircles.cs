using MyApp.Models;
using MyFirstConsoleApp;
using System.Threading.Tasks;

public static class MyCircles
{

    public static void Start(int screenWidth = 80, int screenHeight = 25)
    {
        var myObject = new MyObject(10, 5);

        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
        Console.CursorVisible = false;
        MyUIHelper.WriteCentered("Draw - Press X to exit", screenHeight - 1);
        var distanceOfX = screenWidth / 2;
        var distanceOfY = screenHeight / 2;
        var radius = 3;
        while (true)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("test");
            for (double i = 0; i < 360; i++)
            {
                var x = distanceOfX + radius * 1.9 * Math.Sin(i * Math.PI / 180);
                var y = distanceOfY + radius * Math.Cos(i * Math.PI / 180);
                Console.SetCursorPosition((int)x, (int)y);
                Console.Write("O");
            }
            radius++;
            if (radius >= 10) radius = 1;

            if (Console.KeyAvailable)
            {
                var charPressed = Console.ReadKey(true).Key;
                if (charPressed == ConsoleKey.X)
                {
                    break;
                }
            }


        }
        Console.CursorVisible = true;
        MyUIHelper.WriteCentered("Draw Ended - Press any key to exit", (screenHeight - 1));
        Console.ReadKey(true);

        MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
    }

    public static async Task StartBufferMode2(int screenWidth, int screenHeight)
    {
        MyBuffer myBuffer = new MyBuffer(screenWidth, screenHeight);
        myBuffer.Clear();
        Random random = new Random();
        var myObjects = new List<MyObject>();
        var angle = 0;
        for (int i = 0; i < (screenHeight - 2) / 2; i++)
        {
            var o = new MyObject(0, 0, 15);
            //o.Angle = angle; //random.Next(0, 359);
            o.Angle = random.Next(0, 359);
            angle += 3;
            myObjects.Add(o);
        }

        Console.CursorVisible = false;
        //MatrixScreenSaver(screenHeight, screenWidth);
        var radius = 1;
        var xCenter = (screenWidth - 1) / 2;
        var yCenter = (screenHeight - 1) / 2;
        while (true)
        {
            int radius2 = radius;
            foreach (var obj in myObjects)
            {
                var x = xCenter + radius2 * 2.7 * Math.Sin(obj.Angle * Math.PI / 180);
                var y = yCenter + radius2 * Math.Cos(obj.Angle * Math.PI / 180);
                obj.Position.X = (int)x;
                obj.Position.Y = (int)y;
                obj.DrawToBufferDiff(myBuffer, 'O');
                radius2++;
                obj.Angle+=5;
                if (obj.Angle > 360) obj.Angle = 0;
            }

            myBuffer.SetString(screenWidth / 2, 0, "Press X to exit", "\u001b[97m");
            myBuffer.Render();
            if (Console.KeyAvailable)
            {
                var charPressed = Console.ReadKey(true).Key;
                if (charPressed == ConsoleKey.X)
                {
                    break;
                }
            }
            //Thread.Sleep(80);
        }
        Console.CursorVisible = true;
    }

    private static void DrawCircle(MyObject myObject, int xCenter, int yCenter, int radius, int angle)
    {

    }
}