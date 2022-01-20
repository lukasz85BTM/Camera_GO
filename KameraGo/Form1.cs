using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace KameraGo
{
    public partial class KameraGo : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;

        public KameraGo()
        {
            InitializeComponent();
            bunifuCustomLabel1.Visible = false;
            guna2ResizeForm1.SetResizeForm(this);
        }

        private void KameraGo_Load(object sender, EventArgs e)
        {
            CienOkna.SetShadowForm(this);

            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                MessageBox.Show("Znaleziono przynajmniej jedno urządzenie obrazujące, dodano do listy dostępnych.", "Infromacja WinCAM View", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                guna2ComboBox1.Items.Add(VideoCaptureDevice.Name);
            }
            try
            {
                guna2ComboBox1.SelectedIndex = 0;
                FinalVideo = new VideoCaptureDevice();
            }

            catch
            {
                {
                    MessageBox.Show("Przykro mi ale nie znalazłem żadnych podłączonych urządzeń. Program uruchomi się.. jednak aby uzyskać dostęp do urządzeń należy podłączyć komponent oraz ponownie uruchomić aplikację.", "Infromacja WinCAM View", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap video = (Bitmap)eventArgs.Frame.Clone();
                pictureBox1.Image = video;
            }
            catch
            {
                MessageBox.Show("Podwójny dostęp do użądzenia!", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void KameraGo_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (FinalVideo.IsRunning == true) FinalVideo.Stop();
            }
            catch
            {

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (FinalVideo.IsRunning == true) FinalVideo.Stop();
                FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[guna2ComboBox1.SelectedIndex].MonikerString);
                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);

                bunifuCustomLabel1.Visible = true;
                bunifuCustomLabel1.Text = "REJESTROWANIE OBRAZU";
                FinalVideo.Start();
            }
            catch
            {
                MessageBox.Show("Nie odnaleziono żadnego użądzenia wejściowego, sprawdź Panel Sterowania/Sprzęt i dźwięk/Urządzenia oraz drukarki, po podłączeniu uruchom ponownie program ", "Uwaga", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                bunifuCustomLabel1.Text = "WSTRZYMANY";
                FinalVideo.Stop();
            }
            catch
            {
                MessageBox.Show("Nie można zatrzymać, ponieważ podgląd live jest wyłączony", "Ważny Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_maximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            if (WindowState == FormWindowState.Maximized)
            {
                btn_normalize.Visible = true;
                btn_maximize.Visible = false;
            }
            if (WindowState == FormWindowState.Normal)
            {
                //normalize.Visible = false;
                //bunifuImageButton1.Visible = true;
            }
        }

        private void btn_normalize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btn_normalize.Visible = false;
            btn_maximize.Visible = true;
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (resize_video.Text == "Normalny")
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else 
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }
    }
}
