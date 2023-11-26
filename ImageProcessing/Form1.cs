using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ImageProcessing
{
    public partial class Form1 : Form
    {
        Bitmap loadImage, loadImage2, processedImage;

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

            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
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
                    grey = (byte)((pixel.R + pixel.G + pixel.B) / 3);
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
                for (int y = 0; y < Math.Min(hisdata[x] / 5, 800); y++)
                {
                    mydata.SetPixel(x, 799 - y, Color.Black);
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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            loadImage2 = new Bitmap(openFileDialog2.FileName);
            pictureBox2.Image = loadImage2;
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {

            Subtract(ref loadImage, ref loadImage2, ref processedImage);
            pictureBox3.Image = processedImage;

            btnLoadImageBack.Enabled = false;
            btnLoadImageFront.Enabled = false;
            btnSubtract.Enabled = false;
            pictureBox3.Enabled = false;
            label3.Enabled = false;
            label2.Text = "Original";
            label1.Text = "Processed";
        }

        private void btnLoadImageFront_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
        }

        private void btnLoadImageBack_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void subtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnLoadImageBack.Enabled = true;
            btnLoadImageFront.Enabled = true;
            btnSubtract.Enabled = true;
            pictureBox3.Enabled = true;
            label3.Enabled = true;
            label2.Text = "Front";
            label1.Text = "Background";
        }
        private Device selectedDevice;
        private void openToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Device[] devices = DeviceManager.GetAllDevices();
            if (devices.Length > 0)
            {
                selectedDevice = DeviceManager.GetDevice(0);
                selectedDevice.ShowWindow(pictureBox1);
            }
            else
            {
                MessageBox.Show("No webcam device found.");
            }
        }

        /*private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
                brightness(trackBar1.Value);
        }

        private void brightness(int val) {
            processedImage = new Bitmap(loadImage.Width, loadImage.Height);
            Color pixel;

            for (int x = 0; x < loadImage.Width; x++)
                for (int y = 0; y < loadImage.Height; y++)
                {
                    pixel = loadImage.GetPixel(x, y);
                    if (val >= 0)
                        processedImage.SetPixel(x, y, Color.FromArgb(Math.Min(pixel.R+val, 255), Math.Min(pixel.G + val, 255), Math.Min(pixel.B + val, 255)));
                    else
                        processedImage.SetPixel(x, y, Color.FromArgb(Math.Min(pixel.R - val, 0), Math.Min(pixel.G - val, 0), Math.Min(pixel.B - val, 0)));

                }

            pictureBox2.Image = processedImage;

        }*/


        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }


        public static void Subtract(ref Bitmap a, ref Bitmap b, ref Bitmap result)
        {
            if (b != null)
            {
                result = new Bitmap(b.Width, b.Height);

                Color myGreen = Color.FromArgb(0, 0, 255);
                int greygreen = (myGreen.R + myGreen.G + myGreen.B) / 3;
                int threshold = 5;
                for (int x = 0; x < b.Width; x++)
                {
                    for (int y = 0; y < b.Height; y++)
                    {
                        Color back = a.GetPixel(x, y);
                        Color front = b.GetPixel(x, y);
                        int grey = (front.R + front.G + front.B) / 3;
                        int subtractVal = Math.Abs(greygreen - grey);
                        if (subtractVal < threshold)
                        {
                            result.SetPixel(x, y, back);
                        }
                        else
                        {
                            result.SetPixel(x, y, front);
                        }
                    }

                }
            }
        }
    }
    public class Device
    {
        private const short WM_CAP = 0x400;
        private const int WM_CAP_DRIVER_CONNECT = 0x40a;
        private const int WM_CAP_DRIVER_DISCONNECT = 0x40b;
        private const int WM_CAP_EDIT_COPY = WM_CAP + 30;
        private const int WM_CAP_SET_PREVIEW = 0x432;
        private const int WM_CAP_SET_OVERLAY = 0x433;
        private const int WM_CAP_SET_PREVIEWRATE = 0x434;
        private const int WM_CAP_SET_SCALE = 0x435;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CAP_SEQUENCE = WM_CAP + 62;
        private const int WM_CAP_FILE_SAVEAS = WM_CAP + 23;
        private const int SWP_NOMOVE = 0x20;
        private const int SWP_NOSIZE = 1;
        private const int SWP_NOZORDER = 0x40;
        private const int HWND_BOTTOM = 1;


        //	[DllImport("inpout32.dll", EntryPoint="Out32")]
        //	public static extern void Output(int adress, int value);

        //	[DllImport("inpout32.dll", EntryPoint="Inp32")]
        //	public static extern int Input(int adress);

        [DllImport("avicap32.dll")]
        protected static extern int capCreateCaptureWindowA([MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszWindowName,
            int dwStyle, int x, int y, int nWidth, int nHeight, int hWndParent, int nID);

        [DllImport("user32", EntryPoint = "SendMessageA")]
        protected static extern int SendMessage(int hwnd, int wMsg, int wParam, [MarshalAs(UnmanagedType.AsAny)] object lParam);

        [DllImport("user32")]
        protected static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32")]
        protected static extern bool DestroyWindow(int hwnd);

        int index;
        int deviceHandle;

        public Device()
        {
            //just a simple constructor
        }
        public Device(int index)
        {
            this.index = index;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _version;

        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
        /// <summary>
        /// To Initialize the device
        /// </summary>
        /// <param name="windowHeight">Height of the Window</param>
        /// <param name="windowWidth">Width of the Window</param>
        /// <param name="handle">The Control Handle to attach the device</param>
        public void Init(int windowHeight, int windowWidth, int handle)
        {
            string deviceIndex = Convert.ToString(this.index);
            deviceHandle = capCreateCaptureWindowA(ref deviceIndex, WS_VISIBLE | WS_CHILD, 0, 0, windowWidth, windowHeight, handle, 0);

            if (SendMessage(deviceHandle, WM_CAP_DRIVER_CONNECT, this.index, 0) > 0)
            {
                SendMessage(deviceHandle, WM_CAP_SET_SCALE, -1, 0);
                SendMessage(deviceHandle, WM_CAP_SET_PREVIEWRATE, 0x42, 0);
                SendMessage(deviceHandle, WM_CAP_SET_PREVIEW, -1, 0);

                SetWindowPos(deviceHandle, 1, 0, 0, windowWidth, windowHeight, 6);
            }
        }

        /// <summary>
        /// Shows the webcam preview in the control
        /// </summary>
        /// <param name="windowsControl">Control to attach the webcam preview</param>
        ///                    global::  
        public void ShowWindow(System.Windows.Forms.Control windowsControl)
        {
            Init(windowsControl.Height, windowsControl.Width, windowsControl.Handle.ToInt32());
        }

        /// <summary>
        /// Stop the webcam and destroy the handle
        /// </summary>
        public void Stop()
        {
            SendMessage(deviceHandle, WM_CAP_DRIVER_DISCONNECT, this.index, 0);
            DestroyWindow(deviceHandle);
        }
        public void Sendmessage()
        {
            SendMessage(deviceHandle, WM_CAP_EDIT_COPY, 0, 0);
        }

    }
    public class DeviceManager
    {
        //	[DllImport("inpout32.dll", EntryPoint="Out32")]
        //	public static extern void Output(int adress, int value);

        //	[DllImport("inpout32.dll", EntryPoint="Inp32")]
        //	public static extern int Input(int adress);

        [DllImport("avicap32.dll")]
        protected static extern bool capGetDriverDescriptionA(short wDriverIndex,
            [MarshalAs(UnmanagedType.VBByRefStr)] ref String lpszName,
           int cbName, [MarshalAs(UnmanagedType.VBByRefStr)] ref String lpszVer, int cbVer);

        static ArrayList devices = new ArrayList();

        public static Device[] GetAllDevices()
        {
            String dName = "".PadRight(100);
            String dVersion = "".PadRight(100);

            for (short i = 0; i < 10; i++)
            {
                if (capGetDriverDescriptionA(i, ref dName, 100, ref dVersion, 100))
                {
                    Device d = new Device(i);
                    d.Name = dName.Trim();
                    d.Version = dVersion.Trim();

                    devices.Add(d);
                }
            }

            return (Device[])devices.ToArray(typeof(Device));
        }

        public static Device GetDevice(int deviceIndex)
        {
            return (Device)devices[deviceIndex];
        }
    }
    
}
