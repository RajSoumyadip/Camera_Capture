using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;


namespace CameraCapture
{
    public partial class Form1 : Form
    {
       
        public String path;
        int flag = 1;
        public Form1()
        {
            InitializeComponent();
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox1.WordWrap = true;
            textBox2.WordWrap = true;
            button4.Visible = false;
            textBox3.Visible = true;
            textBox3.Text = "Step 1.Press Start and then choose the path for saving the pics using Browse.";
        }
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        private void button1_Click(object sender, EventArgs e)
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame +=new NewFrameEventHandler( FinalFrame_NewFrame);
            FinalFrame.Start();
            flag = 1;
           
        }

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo Device in CaptureDevice )
            {
                comboBox1.Items.Add(Device.Name);
            }
            comboBox1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FinalFrame.IsRunning==true)
            {
                FinalFrame.Stop();
                button2.Visible = true;
            }
        }

        int j;

        private async void button2_Click(object sender, EventArgs e)
        {
            int j = 0;
            int t = 0;
            if(path == null)
            {
                MessageBox.Show("Please Select Path!!"); 
            }
            if(path != null)
            {
                textBox3.Text = "Keep moving your head LEFT RIGHT TOP DOWN SLOWLY!!";
                button4.Visible = true;

                while((j < 50)&&(flag == 1))
                {
                    t++;
                    button2.Visible = false;
                    textBox1.Visible = true;
                    textBox1.Text = "Please Dont Close The window while we prepare the Data set!!";
                    if (pictureBox1.Image != null)
                    {
                        pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
                        pictureBox1.Image.Save(path + j++ + ".jpeg");
                        await System.Threading.Tasks.Task.Delay(1000);

                    }
                  
                }
                textBox1.Visible = true;
                if(t == 50)
                {
                    textBox1.Text = "Thank you!!";
                }
                else if(t < 5)
                {
                    textBox1.Text = "Please Try Again !!!";
                }
                
                button2.Visible = true;
                button4.Visible = false;
            }
           
            
          

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog()== DialogResult.OK)
            {
                path = folderBrowserDialog.SelectedPath + "\\";
                textBox2.Visible = true;
                textBox2.Text = path;
            }
            textBox3.Text = "Step 2: Press Capture to start the process!!";

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            textBox1.WordWrap = true;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.WordWrap = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (FinalFrame.IsRunning == true)
            {
                FinalFrame.Stop();
                flag = 0;
            }
            button2.Visible = true;
            textBox3.Text = "Press Start and then Capture to Continue!";

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
