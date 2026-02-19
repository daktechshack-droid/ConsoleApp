
using MyFirstConsoleApp;
using LiteDB;
using MyApp.Models;

Console.WriteLine("Learning new things!");

int screenWidth = Console.WindowWidth;
int screenHeight = Console.WindowHeight;
Console.Clear();
Console.SetCursorPosition(0, 0);

MyUIHelper.WriteCentered("My Utility Software", 3);

// Draw a border around the console window
MyUIHelper.DrawBox(0, 0, screenWidth, screenHeight);

using (var db = new LiteDatabase(@"MyData.db"))
{
    var ppl = db.GetCollection<Person>("People");

    ShowMainMenu(screenHeight, ppl);

    //MyUIHelper.ClearBox(10, 10, 10, 5);            
}

MyUIHelper.ClearBox(0, 0, screenWidth, screenHeight);
MyUIHelper.WriteCentered("Press enter key to exit!", screenHeight - 2);
Console.ReadKey(); // Wait for user input before closing the console

static void ShowMainMenu(int screenHeight, ILiteCollection<Person> ppl)
{
    string[] menuItems = new string[]
    {
        "Main Menu",
        "1. Enter your person",
        "2. List all people",
        "3. Sub Menu",
        "Q. Exit"
    };    

    while (true)
    {
        MyUIHelper.BuildMenu(20, 5, 40, 6, menuItems);
        var getSelection = Console.ReadKey(); // Wait for user input before closing the console
        if (getSelection.Key == ConsoleKey.D1)
        {
            MyUIHelper.ClearBox(20, 5, 40, 20);
            MyUIHelper.DrawBox(20, 5, 40, 20);
            var person = new Person
            {
                FirstName = MyUIHelper.PromptReadline(22, 7, "First Name: "),
                LastName = MyUIHelper.PromptReadline(22, 8, "Last Name: "),
                Address = MyUIHelper.PromptReadline(22, 9, "Address: "),
                City = MyUIHelper.PromptReadline(22, 10, "City: "),
                State = MyUIHelper.PromptReadline(22, 11, "State: "),
                Country = MyUIHelper.PromptReadline(22, 12, "Country: "),
                Phone = MyUIHelper.PromptReadline(22, 13, "Phone: ")
            };

            MyUIHelper.WriteAt("Your name: " + person.FirstName + " " + person.LastName, 22, 15);
            ppl.Insert(person);
        }
        if (getSelection.Key == ConsoleKey.D2)
        {
            MyUIHelper.ClearBox(10, 5, 30, 10);
            MyUIHelper.DrawBox(10, 5, 30, 10);
            int i = 0;
            foreach (var person in ppl.FindAll())
            {
                MyUIHelper.WriteAt(person.FirstName + " " + person.LastName, 11, 6 + i);
                i++;
            }
            MyUIHelper.WriteCentered("Press any key to continue!", screenHeight - 2);
            Console.ReadKey(); // Wait for user input before closing the console
            MyUIHelper.ClearBox(10, 5, 30, 10);
            continue;
        }
        if (getSelection.Key == ConsoleKey.D3)
        {
            SubMenu1(40, 8, 30, 5);
            continue;
        }
        if (getSelection.Key == ConsoleKey.Q)
        {
            MyUIHelper.ClearBox(20, 5, 40, 20);
            break;
        }
        MyUIHelper.WriteCentered("Press any key to continue!", screenHeight - 2);
        Console.ReadKey(); // Wait for user input before closing the console
        MyUIHelper.ClearBox(20, 5, 40, 20);
    }

    static void SubMenu1(int x, int y, int width, int height)
    {
        string[] subMenuItems = new string[]
        {
                "Sub Menu",
                "1. IP Address 2",
                "2. Computer Name 2",
                "Q. Exit"
        };
        if(subMenuItems.Count() + 2 > height) height = subMenuItems.Count() + 2; 

        while (true)
        {
            MyUIHelper.BuildMenu(x, y, width, height, subMenuItems);

            var getSelection = Console.ReadKey(); // Wait for user input before closing the console
            MyUIHelper.ClearBox(x, y, width, height);                
            MyUIHelper.DrawBox(x, y, width, height);
            if (getSelection.Key == ConsoleKey.D2)
            {
                MyUIHelper.WriteAt("Computer Name: " + MyHardwareHelper.GetMyComputerName(), x + 1, y + 1);
                MyUIHelper.WriteAt("Press any key!", x + 1, y + height - 2);
            }
            if (getSelection.Key == ConsoleKey.D1)
            {
                var ipAddresses = MyHardwareHelper.GetMyIpAddress();
                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    MyUIHelper.WriteAt("IP Address " + (i + 1) + ": " + ipAddresses[i], x + 1, y + 1 + i);
                }
                MyUIHelper.WriteAt("Press any key!", x + 1, y + height - 2);
            }
            if (getSelection.Key == ConsoleKey.Q)
            {
                break;
            }
            Console.ReadKey(); // Wait for user input before closing the console
        }
        MyUIHelper.ClearBox(x, y, width, height);                
    }
}

