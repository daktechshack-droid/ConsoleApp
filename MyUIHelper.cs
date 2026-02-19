using System;
using System.Reflection;
using System.Text;

namespace MyFirstConsoleApp
{
    public static class MyUIHelper
    {
        public static void DrawBox(int startX, int startY, int width, int height)
        {
            if (width < 2 || height < 2) return;

            // Ensure console outputs UTF-8 so Unicode box-drawing glyphs render
#if true
                Console.OutputEncoding = Encoding.UTF8;
    
                // Unicode box drawing characters (preferred over legacy code page values)
                char topLeft = '\u250C';   // ┌
                char topRight = '\u2510';  // ┐
                char bottomLeft = '\u2514';// └
                char bottomRight = '\u2518';// ┘
                char horizontal = '\u2500'; // ─
                char vertical = '\u2502';   // │
#else                 
                char topLeft = '/';  
                char topRight = '\\'; 
                char bottomLeft = '\\';
                char bottomRight = '/';
                char horizontal = '-'; 
                char vertical = '|';   
#endif
            //topLeft = '*'; topRight = '*'; bottomLeft = '*'; bottomRight = '*'; horizontal = '*'; vertical = '*';

            // Corners
            Console.SetCursorPosition(startX, startY);
            Console.Write(topLeft);

            Console.SetCursorPosition(startX + width - 1, startY);
            Console.Write(topRight);

            Console.SetCursorPosition(startX, startY + height - 1);
            Console.Write(bottomLeft);

            Console.SetCursorPosition(startX + width - 1, startY + height - 1);
            Console.Write(bottomRight);

            // Top and bottom edges
            for (int x = startX + 1; x < startX + width - 1; x++)
            {
                Console.SetCursorPosition(x, startY);
                Console.Write(horizontal);

                Console.SetCursorPosition(x, startY + height - 1);
                Console.Write(horizontal);
            }

            // Left and right edges
            for (int y = startY + 1; y < startY + height - 1; y++)
            {
                Console.SetCursorPosition(startX, y);
                Console.Write(vertical);

                Console.SetCursorPosition(startX + width - 1, y);
                Console.Write(vertical);
            }
        }

        public static void ClearBox(int startX, int startY, int width, int height)
        {
            for (int x = startX; x < startX + width; x++)
            {
                for (int y = startY; y < startY + height; y++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(" ");
                }
            }
        }

        public static void WriteAt(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public static void WriteCentered(string text, int y)
        {
            int x = (Console.WindowWidth - text.Length) / 2;
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        public static void BuildMenu(int x, int y, int width, int height, string[] menuItems)
        {
            if(menuItems == null || menuItems.Count() == 0) return; // Ensure menuItems is not null to avoid exceptions
            if(menuItems.Count() + 2 > height) height = menuItems.Count() + 2; // Ensure menuItems is not null to avoid exceptions
            MyUIHelper.ClearBox(x, y, width, height);                
            MyUIHelper.DrawBox(x, y, width, height);

            foreach (var item in menuItems)
            {
                MyUIHelper.WriteAt(item, x + 1, y + 1);
                y++;
            }
        }        

        public static string ReadAtPosition(int left, int top, int maxLength)
        {
            StringBuilder input = new StringBuilder();
            int currentLeft = left;

            while (true)
            {
                Console.SetCursorPosition(currentLeft, top);
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Tab) break;

                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    currentLeft--;
                    Console.SetCursorPosition(currentLeft, top);
                    Console.Write(" "); // Clear character
                }
                else if (!char.IsControl(key.KeyChar) && input.Length < maxLength)
                {
                    input.Append(key.KeyChar);
                    Console.SetCursorPosition(currentLeft, top);
                    Console.Write(key.KeyChar);
                    currentLeft++;
                }
            }
            return input.ToString();
        }

        public static string PromptInput(int x, int y, string prompt)
        {
            MyUIHelper.WriteAt(prompt, x, y);
            return MyUIHelper.ReadAndEditAt(x + prompt.Length, y, 20);
        }

        public static string? PromptReadline(int x, int y, string prompt)
        {
            MyUIHelper.WriteAt(prompt, x, y);
            Console.SetCursorPosition(x + prompt.Length, y);
            return Console.ReadLine();
        }

        public static string ReadAndEditAt(int left, int top, int maxLength)
        {
            var input = new StringBuilder();
            int index = 0; // Current position in the string

            while (true)
            {
                Console.SetCursorPosition(left + index, top);
                var keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        return input.ToString();

                    case ConsoleKey.LeftArrow:
                        if (index > 0) index--;
                        break;

                    case ConsoleKey.RightArrow:
                        if (index < input.Length) index++;
                        break;

                    case ConsoleKey.Backspace:
                        if (index > 0)
                        {
                            input.Remove(index - 1, 1);
                            index--;
                            RefreshLine(left, top, input, maxLength);
                        }
                        break;

                    case ConsoleKey.Delete:
                        if (index < input.Length)
                        {
                            input.Remove(index, 1);
                            RefreshLine(left, top, input, maxLength);
                        }
                        break;

                    default:
                        if (!char.IsControl(keyInfo.KeyChar) && input.Length < maxLength)
                        {
                            input.Insert(index, keyInfo.KeyChar);
                            index++;
                            RefreshLine(left, top, input, maxLength);
                        }
                        break;
                }
            }
        }

        static void RefreshLine(int left, int top, StringBuilder sb, int max)
        {
            Console.SetCursorPosition(left, top);
            // Print current string + space to clear old chars, capped at maxLength
            Console.Write(sb.ToString().PadRight(max).Substring(0, max));
        }

    }
}