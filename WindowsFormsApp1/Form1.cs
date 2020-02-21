using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO.Ports;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            serialPort1.BaudRate = 9600;
        }

        private FilterInfoCollection webcam;

        private VideoCaptureDevice cam;

        int renkKoduAl = 0;

        Color defaultColor = Color.FromArgb(0, 0, 0);

        Color color2;

        private void SetColor(Color color)
        {
            try
            {
                serialPort1.WriteLine(color.R.ToString());
                serialPort1.WriteLine(",");
                serialPort1.WriteLine(color.G.ToString());
                serialPort1.WriteLine(",");
                serialPort1.WriteLine(color.B.ToString());
                serialPort1.WriteLine(",");
            }
            catch (Exception) { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox2.DataSource = SerialPort.GetPortNames();

                webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                foreach (FilterInfo item in webcam)
                    comboBox1.Items.Add(item.Name);

                comboBox1.SelectedIndex = 0;
            }
            catch (Exception) { }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
                cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
                cam.Start();
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cam.IsRunning)
                {
                    cam.Stop();
                    pictureBox1.Image = null;
                }
            }
            catch (Exception) { }
        }

        void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bmp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog swf = new SaveFileDialog();
                swf.Filter = "(*.jpg)|*.jpg|Bitma*p(*.bmp)|*.bmp";
                DialogResult dialog = swf.ShowDialog();

                if (dialog == DialogResult.OK)
                    pictureBox1.Image.Save(swf.FileName);
            }
            catch (Exception) { }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (renkKoduAl == 1)
                {
                    color2 = ((Bitmap)pictureBox1.Image).GetPixel(e.X, e.Y);
                    string rgb = string.Format("R:{0} G:{1} B:{2}", color2.R, color2.G, color2.B);
                    textBox1.Text = rgb;
                    panel4.BackColor = color2;
                    pictureBox1.Cursor = Cursors.Default;
                    renkKoduAl = 0;
                }
            }
            catch (Exception)
            {
                pictureBox1.Cursor = Cursors.Default;
                renkKoduAl = 0;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Cursor = Cursors.Cross;
                renkKoduAl = 1;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (button5.Text == "Bağlantı Aç")
                {
                    serialPort1.PortName = comboBox2.SelectedItem.ToString();
                    serialPort1.Open();
                    button5.Text = "Bağlantı Kapat";
                    label2.Text = "Arduino Bağlantı Açık";
                    label2.ForeColor = Color.Green;
                }
                else
                {
                    serialPort1.Close();
                    button5.Text = "Bağlantı Aç";
                    label2.Text = "Arduino Bağlantı Kapalı";
                    label2.ForeColor = Color.Red;
                }
            }
            catch (Exception) { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetColor(color2);
        }
        
        private void button7_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                SetColor(colorDialog1.Color);
        }
    }
}
