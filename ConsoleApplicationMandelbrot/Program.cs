using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace ConsoleApplicationMandelbrot
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            Console.WriteLine(DateTime.Now.ToLongTimeString()+ ":" + DateTime.Now.Millisecond);
            Bitmap finalBitmap = new Bitmap(1024*25, 800*25, PixelFormat.Format24bppRgb);
            new MandelCalcConsole().GenerateBitmap(finalBitmap);
            finalBitmap.Save("mandelbrot.png");

            DateTime end = DateTime.Now;
            Console.WriteLine("Finished at: " + DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond);
            Console.Write("Total time: ");
            Console.WriteLine(end-start);
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
