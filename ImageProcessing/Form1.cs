using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap loadImage, processedImage;
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            loadImage = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = loadImage;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadImage.Width, loadImage.Height);
            Color pixel;

            for(int x = 0; x<loadImage.Width; x++)
                for(int y = 0; y<loadImage.Height; y++)
                {
                    pixel = loadImage.GetPixel(x, y);
                    processedImage.SetPixel(x, y, pixel);
                }

            pictureBox2.Image = processedImage;
        }

        private void greyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadImage.Width, loadImage.Height);
            Color pixel;
            int grey;
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    pixel = loadImage.GetPixel(x, y);
                    grey = (byte) ((pixel.R + pixel.G + pixel.B)/3);
                    processedImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }

            pictureBox2.Image = processedImage;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadImage.Width, loadImage.Height);
            Color pixel;
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    pixel = loadImage.GetPixel(x, y);
                    processedImage.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.G, 255 - pixel.B));
                }

            pictureBox2.Image = processedImage;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadImage.Width, loadImage.Height);
            Color pixel;
            int grey;
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    pixel = loadImage.GetPixel(x, y);
                    grey = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    processedImage.SetPixel(x, y, Color.FromArgb(grey, grey, grey));
                }

            int[] hisdata = new int[256];
            Color sample;
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    sample = processedImage.GetPixel(x, y);
                    hisdata[sample.R]++;
                }

            Bitmap mydata = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 800; y++)
                {
                    mydata.SetPixel(x, y, Color.White);
                }

            for (int x = 0; x < 256; x++)
                for (int y = 0; y < Math.Min(hisdata[x]/5,800); y++)
                {
                    mydata.SetPixel(x, 799-y, Color.Black);
                }

            pictureBox2.Image = mydata;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processedImage = new Bitmap(loadImage.Width, loadImage.Height);
            Color pixel;
            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    pixel = loadImage.GetPixel(x, y);
                    int sepiaR = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int sepiaG = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int sepiaB = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    sepiaR = Math.Min(255, Math.Max(0, sepiaR));
                    sepiaG = Math.Min(255, Math.Max(0, sepiaG));
                    sepiaB = Math.Min(255, Math.Max(0, sepiaB));

                    processedImage.SetPixel(x, y, Color.FromArgb(sepiaR, sepiaG, sepiaB));
                }

            pictureBox2.Image = processedImage;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPEG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.ShowDialog();
            
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
            processedImage.Save(saveFileDialog1.FileName);
            
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
    }
}
