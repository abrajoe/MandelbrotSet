using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplicationMandelbrot
{

    public class MandelCalcConsole
    {
        //readonly float MaxValueExtent; 
        readonly double MaxColor;
        readonly double ContrastValue; 
        //readonly int MaxIterations;
        //readonly double MaxNorm;

        public MandelCalcConsole()
        {
            //MaxValueExtent = 1.5f;
            MaxColor = 256;
            ContrastValue = 0.2;
            //MaxIterations = 10000;
            //MaxNorm = MaxValueExtent * MaxValueExtent;
        }

        public void GenerateBitmap(Bitmap bitmap)
        {

            float[,] workload = new float[bitmap.Height, bitmap.Width];

            int numThreads = 8;
            if (bitmap.Height / numThreads < 1)
                numThreads = bitmap.Height; //es gab mehr threads als das bild breit ist

            int threadRows = bitmap.Height / numThreads; //the number of rows one thread processes

            Thread[] preciousLittleThreadies = new Thread[numThreads-1];
            for (int i = 0; i < numThreads; i++)
            {
                int threadStartRow = i * threadRows;
                int threadEndRow = threadStartRow + threadRows;
                object[] args = new object[] { threadStartRow, threadEndRow, workload, bitmap.Height, bitmap.Width };
                if (i != numThreads - 1)
                {
                    preciousLittleThreadies[i] = new Thread(new ParameterizedThreadStart(CalcRow));
                    preciousLittleThreadies[i].Start(args);
                }
                else
                {
                    //Run the last part on the main thread
                    CalcRow(args);
                }
            }

            //legen...wait for it
            for (int i = 0; i < numThreads-1; i++)
            {
                int threadStartRow = i * threadRows;
                int threadEndRow = threadStartRow + threadRows;
                preciousLittleThreadies[i].Join();

                for (int j = threadStartRow; j < threadEndRow; j++)
                {
                    for (int k = 0; k < bitmap.Width; k++)
                    {
                        bitmap.SetPixel(k, j, GetColor(workload[j, k]));
                    }
                }
            }
            //...dary

            //for (int i = 0; i < bitmap.Height; i++)
            //{
            //    for (int j = 0; j < bitmap.Width; j++)
            //    {
            //        bitmap.SetPixel(j, i, GetColor(workload[i,j]));
            //    }
            //}
        }
   
        public void CalcRow(object args)
        {
            object[] argsA = (object[])args;
            int threadStartRow = (int)argsA[0];
            int threadEndRow = (int)argsA[1];
            float[,] workload = (float[,])argsA[2];
            //double[,] tempWorkload = new double[workload.GetUpperBound(0)+1, workload.GetUpperBound(1)+1];
            int height = (int)argsA[3];
            int width = (int)argsA[4];

            float scale = 2 * 1.5f / Math.Min(width, height);

            for (int i = threadStartRow; i < threadEndRow; i++)
            {
                float y = (height / 2 - i) * scale;
                for (int j = 0; j < width; j++)
                {
                    float x = (j - width / 2) * scale;
                    float color = CalcMandelbrotSetColor(new ComplexNumber(x, y));
                    workload[i, j] = color;
                }
            }
            //workload = tempWorkload;
        }

        private float CalcMandelbrotSetColor(ComplexNumber c)
        {
            // from http://en.wikipedia.org/w/index.php?title=Mandelbrot_set

            int iteration = 0;
            ComplexNumber z = new ComplexNumber();
            do
            {
                z = z * z + c;
                iteration++;
            } while (z.Norm() < 2.25f && iteration < 10000);

            if (iteration < 10000)
                return (float)iteration / 10000;
            else
                return 0; // black
        }

        private Color GetColor(double value)
        {
            return Color.FromArgb(0, 0, (int)(MaxColor * Math.Pow(value, ContrastValue)));
        }


        struct ComplexNumber
        {
            public float Re;
            public float Im;

            public ComplexNumber(float re, float im)
            {
                this.Re = re;
                this.Im = im;
            }

            public static ComplexNumber operator +(ComplexNumber x, ComplexNumber y)
            {
                return new ComplexNumber(x.Re + y.Re, x.Im + y.Im);
            }

            public static ComplexNumber operator *(ComplexNumber x, ComplexNumber y)
            {
                return new ComplexNumber(x.Re * y.Re - x.Im * y.Im,
                    x.Re * y.Im + x.Im * y.Re);
            }

            public float Norm()
            {
                return Re * Re + Im * Im;
            }
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