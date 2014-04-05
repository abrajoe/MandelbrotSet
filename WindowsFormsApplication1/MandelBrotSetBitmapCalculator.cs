using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{

    public static class MandelBrotSetBitmapCalculator
    {
        readonly private static double MaxValueExtent = 1.5f;
        readonly private static double MaxColor = 256;
        readonly private static double ContrastValue = 0.2;
        readonly private static int MaxIteration = 10000;
        readonly private static double MaxNorm = 2.25f;


        public static void GenerateBitmap(Bitmap bitmap)
        {
            
            double[,] workload = new double[bitmap.Height, bitmap.Width];

            int numThreads = 8;
            if (bitmap.Height / numThreads < 1)
                numThreads = bitmap.Height; //es gab mehr threads als das bild breit ist

            int threadRows = bitmap.Height / numThreads; //the number of rows one thread processes

            Thread[] preciousLittleThreadies = new Thread[numThreads];
            


            for (int i = 0; i < numThreads; i++)
            {
                preciousLittleThreadies[i] = new Thread(new ParameterizedThreadStart(CalcRow));
                int threadStartRow = i * threadRows;
                int threadEndRow = threadStartRow + threadRows;
                object[] args = new object[] { threadStartRow, threadEndRow, workload, bitmap.Height, bitmap.Width };
                preciousLittleThreadies[i].Start(args);
            }

            //legen...wait for it
            foreach (var t in preciousLittleThreadies)
            {
                t.Join();
            }
            //...dary

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    bitmap.SetPixel(j, i, GetColor(workload[i, j], MaxColor));
                }
            }
        }

        public static void CalcRow(object args)
        {
            object[] argsA = (object[])args;
            int threadStartRow = (int)argsA[0];
            int threadEndRow = (int)argsA[1];
            double[,] workload = (double[,])argsA[2];
            //double[,] tempWorkload = new double[workload.GetUpperBound(0)+1, workload.GetUpperBound(1)+1];
            int height = (int)argsA[3];
            int width = (int)argsA[4];

            double scale = 2 * MaxValueExtent / Math.Min(width, height);

            for (int i = threadStartRow; i < threadEndRow; i++)
            {
                double y = (height / 2 - i) * scale;
                for (int j = 0; j < width; j++)
                {
                    double x = (j - width / 2) * scale;
                    double color = CalcMandelbrotSetColor(new ComplexNumber(x, y));
                    workload[i, j] = color;
                }
            }
            //workload = tempWorkload;
        }

        private static double CalcMandelbrotSetColor(ComplexNumber complex_c)
        {
            return MandelBrotSetCalculator.CalculateMandelbrotSetColor(complex_c, MaxIteration, MaxNorm);
        }

  

        private static Color GetColor(double value, double MaxColor)
        {
            return Color.FromArgb(0, 0, (int)(MaxColor * Math.Pow(value, ContrastValue)));
        }
    }
}
#region unused
//double scale = 2 * MaxValueExtent / Math.Min(bitmap.Width, bitmap.Height);
//for (int i = 0; i < bitmap.Height; i++)
//{
//    double y = (bitmap.Height / 2 - i) * scale;
//    for (int j = 0; j < bitmap.Width; j++)
//    {
//        double x = (j - bitmap.Width / 2) * scale;
//        double color = CalcMandelbrotSetColor(new ComplexNumber(x, y));
//        bitmap.SetPixel(j, i, GetColor(color));
//    }
//}
#endregion