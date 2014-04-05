using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class MandelBrotSetCalculator
    {
        public static double CalculateMandelbrotSetColor(ComplexNumber complex_c, int MaxIterations, double MaxNorm)
        {
            // from http://en.wikipedia.org/w/index.php?title=Mandelbrot_set

            int iteration = 0;
            ComplexNumber complex_z = new ComplexNumber();
            do
            {
                complex_z = complex_z * complex_z + complex_c;
                iteration++;
            }
            while (complex_z.Norm() < MaxNorm && iteration < MaxIterations);

            if (iteration < MaxIterations)
                return (double)iteration / MaxIterations;
            else
                return 0; // black
        }

    }
}
