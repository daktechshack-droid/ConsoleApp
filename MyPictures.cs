using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Color = System.Drawing.Color;

namespace MyApp
{
    public static class MyPictures
    {
        static string[] grayFade12 =
        {
            "\u001b[38;5;15m",  // Pure White
            "\u001b[38;5;255m", // Grey 93%
            "\u001b[38;5;252m", // Grey 82%
            "\u001b[38;5;250m", // Grey 74%
            "\u001b[38;5;248m", // Grey 66%
            "\u001b[38;5;246m", // Grey 58%
            "\u001b[38;5;244m", // Grey 50%
            "\u001b[38;5;242m", // Grey 42%
            "\u001b[38;5;240m", // Grey 35%
            "\u001b[38;5;238m", // Grey 27%
            "\u001b[38;5;235m", // Grey 15%
            "\u001b[38;5;232m"  // Near Black
        };

        static string[] greenFade12 =
        {
            "\u001b[38;5;46m",  // Bright Lime Green
            "\u001b[38;5;118m", // Light Green
            "\u001b[38;5;82m",  // Spring Green
            "\u001b[38;5;76m",  // Mid-Light Green
            "\u001b[38;5;70m",  // Medium Green
            "\u001b[38;5;64m",  // Darker Green
            "\u001b[38;5;28m",  // Forest Green
            "\u001b[38;5;22m",  // Deep Forest Green
            "\u001b[38;5;238m", // Green-tinted Dark Grey
            "\u001b[38;5;236m", // Dark Moss
            "\u001b[38;5;234m", // Near-black Green
            "\u001b[38;5;232m"  // Black (Grayscale base)
        };
        public static async Task StartBufferMode(int screenWidth, int screenHeight)
        {
            MyBuffer myBuffer = new MyBuffer(screenWidth, screenHeight);
            myBuffer.Clear();
            var extensions = new[] { ".png", ".jpg", ".jpeg" };
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "\\images\\", "*.*")
                                 .Where(f => extensions.Contains(Path.GetExtension(f).ToLower())).ToList();

            var i = 0;
            Console.CursorVisible = false;
            while (true)
            {
                string filename = files[i];
                ReadFromFile(filename, screenWidth, screenHeight, myBuffer);
                //ReadFromFileSixLabors(filename, screenWidth, screenHeight, myBuffer);

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
                Thread.Sleep(1000);
                myBuffer.Clear();
                i++; if (i > files.Count) i = 0;
            }
            Console.CursorVisible = true;
        }

        public static void ReadFromFile(string fileName, int screenWidth, int screenHeight, MyBuffer myBuffer)
        {
            if (!File.Exists(fileName)) return;
            var newSize = new System.Drawing.Size(screenWidth, screenHeight);
            Bitmap orig = new Bitmap(fileName);

            Bitmap bmp = new Bitmap(orig, newSize);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixelColor = bmp.GetPixel(i, j);
                    string ramp = " .:-'\\\"^*+!i|=?X08&WM#@\";"; // Simple 10-char ramp

                    int brightness = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);
                    int index = (brightness * (ramp.Length - 1)) / 255;

                    // 3. Get the color code (Mapping 0-255 to 0-11)
                    int colorIndex = (brightness * (grayFade12.Length - 1)) / 255;
                    string colorCode = grayFade12[grayFade12.Length - 1 - colorIndex];

                    char asciiChar = ramp[index];                    // Process the pixel (e.g., change color, check values)
                    //myBuffer.SetChar(i, j, asciiChar, "");
                    myBuffer.SetChar(i, j, 'o', colorCode);
                    // To set a pixel, use bmp.SetPixel(i, j, newColor);
                }
            }
        }

        public static void ReadFromFileSixLabors(string fileName, int screenWidth, int screenHeight, MyBuffer myBuffer)
        {
            if (!File.Exists(fileName)) return;
            using Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(fileName);
            image.Mutate(x => x.Resize(screenWidth, screenHeight));

            string ramp = " .:-'\"^*+!i|=?X08&WM#@";

            // 2. Iterate using ProcessPixelRows (efficient for Linux/Cross-platform)
            image.ProcessPixelRows(accessor =>
            {
                for (int y = 0; y < accessor.Height; y++)
                {
                    // Get a span of pixels for the current row
                    Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                    for (int x = 0; x < pixelRow.Length; x++)
                    {
                        Rgba32 pixelColor = pixelRow[x];

                        // Luminance calculation
                        int brightness = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);

                        // Map to ASCII
                        int index = (brightness * (ramp.Length - 1)) / 255;
                        char asciiChar = ramp[index];

                        // 3. Get the color code (Mapping 0-255 to 0-11)
                        int colorIndex = (brightness * (grayFade12.Length - 1)) / 255;
                        string colorCode = grayFade12[grayFade12.Length - 1 - colorIndex];
                        // Update your buffer
                        myBuffer.SetChar(x, y, 'O', colorCode);
                    }
                }
            });
        }


        public static void ReadFromFile2(string fileName)
        {
            // ...

            //Bitmap bmp = new Bitmap(fileName);

            //// Lock the bitmap bits
            //Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            //BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            //// Get the address of the first line
            //IntPtr ptr = bmpData.Scan0;

            //// Declare an array to hold the bytes of the bitmap. 
            //// This example assumes a 32bpp (ARGB) image for simplicity.
            //int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            //byte[] rgbValues = new byte[bytes];

            //// Copy the RGB values into the array
            //System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            //// Iterate over the pixels
            //// 4 bytes per pixel for 32bpp, in BGRA order
            //for (int i = 0; i < rgbValues.Length; i += 4)
            //{
            //    // Get color components (example for BGRA format)
            //    byte blue = rgbValues[i];
            //    byte green = rgbValues[i + 1];
            //    byte red = rgbValues[i + 2];
            //    byte alpha = rgbValues[i + 3];

            //    // Perform operations (e.g., invert colors)
            //    rgbValues[i] = (byte)(255 - blue);
            //    rgbValues[i + 1] = (byte)(255 - green);
            //    rgbValues[i + 2] = (byte)(255 - red);
            //    // Alpha channel usually remains unchanged

            //    // Or process the pixel values as needed
            //}

            //// Copy the modified bytes back to the bitmap
            //System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            //// Unlock the bits.
            //bmp.UnlockBits(bmpData);

            //// Save the modified image
            //bmp.Save(@"C:\path\to\your\modified_image.png", ImageFormat.Png);

        }

    }
}
