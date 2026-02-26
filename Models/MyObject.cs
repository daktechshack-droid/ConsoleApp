using MyApp.Models;

public class MyObject
{
    public MyPoint Position { get; set; }
    public List<MyObjectChar> Trail { get; set; }

    public int TrailLength { get; set; }

    public MyObject(int x, int y, int trailLength = 10)
    {
        Position = new MyPoint(x, y);
        Trail = new List<MyObjectChar>();
        TrailLength = trailLength;
        for (int i = 0; i < TrailLength; i++)
        {
            Trail.Add(new MyObjectChar(new MyPoint(x, y), '.'));
        }
    }

    public void AddTrailPoint(MyObjectChar point)
    {
        TrailLength++;
        Trail.Add(point);
        if (Trail.Count > TrailLength)
        {
            Trail.RemoveAt(0);
        }
    }
    
    static ConsoleColor[] greenFade = { 
            ConsoleColor.Green, 
            ConsoleColor.Green, 
            ConsoleColor.Green, 
            ConsoleColor.DarkGreen, 
            ConsoleColor.DarkGreen, 
            ConsoleColor.DarkGreen, 
            ConsoleColor.DarkGreen, 
            ConsoleColor.DarkGreen, 
            ConsoleColor.DarkGreen, 
            ConsoleColor.DarkGray,
        };    

    static ConsoleColor[] greenFade2 = { 
            ConsoleColor.Red, 
            ConsoleColor.DarkMagenta,
            ConsoleColor.Yellow,
            ConsoleColor.DarkYellow, 
            ConsoleColor.Cyan,
            ConsoleColor.DarkCyan,
            ConsoleColor.Green, 
            ConsoleColor.DarkGreen, 
            ConsoleColor.Gray,
            ConsoleColor.DarkGray,
        };    

    Random rnd = new Random();
    public void Draw()
    {
        Console.SetCursorPosition((int)Trail[0].Position.X, (int)Trail[0].Position.Y);
        Console.Write(' ');
        
        Trail.RemoveAt(0);
        Trail.Add(new MyObjectChar(Position, (char)rnd.Next(32, 127)));
        int g = greenFade.Length - 1;
        foreach (var point in Trail)
        {
            Console.SetCursorPosition((int)point.Position.X, (int)point.Position.Y);
            Console.ForegroundColor = greenFade[g];
            Console.Write(point.Character);
            g--;
            if (g < 0) g = 0;
        }
        Console.ResetColor();
        Console.SetCursorPosition((int)Position.X, (int)Position.Y);
        Console.Write("O");
    }
}