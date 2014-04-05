using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MandelbrotSetForm();
        }

        
        Thread thread;


        public void MandelbrotSetForm()
        {
            // form creation
            this.Text = "Mandelbrot Set Drawing";
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ClientSize = new Size(640, 640);
            this.Load += new System.EventHandler(this.MainForm_Load);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            thread = new Thread(thread_Proc) { IsBackground = true };
            thread.Start(this.ClientSize);
        }

        void thread_Proc(object args)
        {
            // start from small image to provide instant display for user
            //Size size = (Size)args;
            //int width = 16;
            //while (width * 2 < size.Width)
            //{
            //    int height = width * size.Height / size.Width;
            //    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            //    new MandelCalc().GenerateBitmap(bitmap);
            //    this.BeginInvoke(new SetNewBitmapDelegate(SetNewBitmap), bitmap);
            //    width *= 2;
            //    // Thread.Sleep(200);
            //}
            // then generate final image
            //Bitmap finalBitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format24bppRgb);
            Bitmap finalBitmap = new Bitmap(1024, 800, PixelFormat.Format24bppRgb);
            MandelBrotSetBitmapCalculator.GenerateBitmap(finalBitmap);
            this.BeginInvoke(new SetNewBitmapDelegate(SetNewBitmap), finalBitmap);
            //finalBitmap.Save("mandelbrot.png");
            // Standby
            //Application.SetSuspendState(PowerState.Suspend, true, true);
        }

        void SetNewBitmap(Bitmap image)
        {
            if (this.BackgroundImage != null)
                this.BackgroundImage.Dispose();
            this.BackgroundImage = image;
        }

        delegate void SetNewBitmapDelegate(Bitmap image);


        

    }
}
