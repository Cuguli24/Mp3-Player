using NAudio.Wave;
using System.Runtime.InteropServices;

namespace MP3_Player_2
{
    public partial class Form1 : Form
    {

        WaveOutEvent outEvent;
        AudioFileReader Reader;


        string ComputerName = Environment.UserName;
        string directionPath;
        string content;
        string[] musics = new string[1024];

        bool çalýþýyor = false;
        bool trackBasýlý = false;

        int seçiliÝndex;
        int totalMusics;

        float sesvalue = 0.5f;

        double toplamsüre;

        int dakika;
        int saniye;


        public Form1()
        {
            InitializeComponent();
        }


        private void ögeseç()
        {
            string seçilenÖge = musics[seçiliÝndex].ToString();
            isim.Text = seçilenÖge;
            seçilenÖge = $"C:\\Users\\{ComputerName}\\Music" + "\\" + seçilenÖge;
            outEvent = new WaveOutEvent();
            Reader = new AudioFileReader(seçilenÖge);
            outEvent.Init(Reader);
            toplamsüre = Reader.TotalTime.TotalSeconds;
            trackbar.Maximum = Convert.ToInt32(toplamsüre);
            outEvent.Play();
            timer.Start();
            double floatDakika = toplamsüre / 60;
            dakika = int.Parse(floatDakika.ToString().Remove(floatDakika.ToString().IndexOf(",")));
            saniye = Convert.ToInt32(toplamsüre % 60);
            if (saniye < 10)
            {
                süre.Text = dakika.ToString() + ":0" + saniye.ToString();
            }
            else
            {
                süre.Text = dakika.ToString() + ":" + saniye.ToString();
            }
            trackbar.Value = 0;
            þuan.Text = "0:00";
            guna2PictureBox1.Image = Resource1.Pause;
            Logo.Image = Resource1.musicgif;
            çalýþýyor = true;
        }

        private void durdur()
        {
            outEvent.Pause();
            guna2PictureBox1.Image = Resource1.Play;
            Logo.Image = Resource1.CUGULI__1_;
            çalýþýyor = false;
            timer.Stop();
        }

        private void çalýþtýr()
        {
            outEvent.Play();
            guna2PictureBox1.Image = Resource1.Pause;
            Logo.Image = Resource1.musicgif;
            çalýþýyor = true;
            timer.Start();
        }

        private void randomseç()
        {
            Random random = new Random();
            int randomSayý = random.Next(0, totalMusics);
            while (randomSayý == seçiliÝndex)
            {
                randomSayý = random.Next(0, totalMusics);
            }
            outEvent.Dispose();
            Reader.Dispose();
            seçiliÝndex = randomSayý;
            ögeseç();
            çalýþtýr();
        }

        private void loop()
        {
            outEvent.Dispose();
            Reader.Dispose();
            ögeseç();
            çalýþtýr();
        }


        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void kapat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            directionPath = $"C:\\Users\\{ComputerName}\\Music";
            content = $"C:\\Users\\{ComputerName}\\Music\\Musicals.txt";

            if (!Directory.Exists(directionPath))
            {
                Directory.CreateDirectory(directionPath);
            }

            if (!File.Exists(content))
            {

                FileStream fileStream = File.Create(content);
                fileStream.Close();
            }

            totalMusics = File.ReadLines(content).Count();

            if (totalMusics > 0)
            {
                musics = File.ReadAllLines(content);

                for (int i = 0; i <= totalMusics - 1; i++)
                {
                    Müzikler.Items.Add(musics[i]);
                }
            }
        }

        private void addmusic_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Cuguli Müzik | Select a MP3 file";
            openFileDialog.Filter = "MP3 Files|*.mp3";

            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                string musicPath = openFileDialog.FileName;
                string musicName = Path.GetFileName(musicPath);

                string copyPath = directionPath + "\\" + musicName;

                if (!File.Exists(copyPath))
                {
                    File.Copy(musicPath, copyPath);
                    File.AppendAllText(content, musicName + "\n");

                    MessageBox.Show("Music Added", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Müzikler.Items.Add(musicName);
                }
                else
                {
                    MessageBox.Show("This File Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                musics = File.ReadAllLines(content);
            }
        }

        private void Müzikler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (outEvent != null && Reader != null)
            {
                outEvent.Dispose();
                Reader.Dispose();
                guna2PictureBox1.Image = Resource1.Play;
                Logo.Image = Resource1.CUGULI__1_;
                çalýþýyor = false;

            }
            if (Müzikler.SelectedItem != null)
            {
                seçiliÝndex = Müzikler.SelectedIndex;
                ögeseç();
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (outEvent != null && !çalýþýyor)
            {
                çalýþtýr();
            }

            else if (outEvent != null && çalýþýyor)
            {
                durdur();
            }
        }

        private void önceki_Click(object sender, EventArgs e)
        {
            timer.Stop();
            trackbar.Value = 0;
            þuan.Text = "0:00";
            if (outEvent != null && Reader != null)
            {
                if (Reader.CurrentTime.TotalSeconds <= 5)
                {
                    outEvent.Dispose();
                    Reader.Dispose();
                    guna2PictureBox1.Image = Resource1.Play;
                    Logo.Image = Resource1.CUGULI__1_;
                    çalýþýyor = false;


                    if (seçiliÝndex > 0)
                    {
                        seçiliÝndex--;
                        ögeseç();

                    }
                    else
                    {
                        seçiliÝndex = musics.Length - 1;
                        ögeseç();
                    }
                }

                else
                {
                    timer.Stop();
                    trackbar.Value = 0;
                    Reader.CurrentTime = TimeSpan.FromSeconds(trackbar.Value);
                    þuan.Text = "0:00";
                    timer.Start();
                }
            }
        }

        private void sonraki_Click(object sender, EventArgs e)
        {
            timer.Stop();
            trackbar.Value = 0;
            þuan.Text = "0:00";
            if (outEvent != null && Reader != null)
            {
                outEvent.Dispose();
                Reader.Dispose();
                guna2PictureBox1.Image = Resource1.Play;
                Logo.Image = Resource1.CUGULI__1_;
                çalýþýyor = false;


                if (seçiliÝndex < totalMusics - 1)
                {
                    seçiliÝndex++;
                    ögeseç();

                }
                else
                {
                    seçiliÝndex = 0;
                    ögeseç();
                }
            }
        }

        private void trackbar_Scroll(object sender, ScrollEventArgs e)
        {
            if (outEvent != null && Reader != null)
            {
                outEvent.Dispose();
                Reader.Dispose();
                guna2PictureBox1.Image = Resource1.Play;
                Logo.Image = Resource1.CUGULI__1_;
                çalýþýyor = false;
                timer.Stop();

                float trackvalue = trackbar.Value;
                if (trackvalue == 0)
                {
                    return;
                }
                else if (trackvalue % 60 == 0)
                {
                    float floatDakika = trackvalue / 60;
                    þuan.Text = dakika + ":00";
                }
                else
                {
                    float floatDakika = trackvalue / 60;
                    dakika = int.Parse(floatDakika.ToString().Remove(floatDakika.ToString().IndexOf(",")));
                    saniye = Convert.ToInt32(trackvalue % 60);
                    string saniye2 = saniye.ToString();
                    if (saniye < 10)
                    {
                        saniye2 = "0" + saniye.ToString();
                    }
                    þuan.Text = dakika.ToString() + ":" + saniye2;
                }
            }
        }

        private void trackbar_MouseUp(object sender, MouseEventArgs e)
        {
            if (outEvent != null && e.Button==MouseButtons.Left)
            {
                trackBasýlý = false;
                string seçilenÖge = musics[seçiliÝndex].ToString();
                isim.Text = seçilenÖge;
                seçilenÖge = $"C:\\Users\\{ComputerName}\\Music" + "\\" + seçilenÖge;
                outEvent = new WaveOutEvent();
                Reader = new AudioFileReader(seçilenÖge);
                outEvent.Init(Reader);
                Reader.CurrentTime = TimeSpan.FromSeconds(trackbar.Value);
                outEvent.Play();
                guna2PictureBox1.Image = Resource1.Pause;
                Logo.Image = Resource1.musicgif;
                çalýþýyor = true;
                timer.Start();
                if (trackbar.Value == Convert.ToInt32(toplamsüre))
                {
                    if (repeat.Checked)
                    {
                        loop();
                    }
                    else
                    {
                        randomseç();
                    }
                }
            }
        }

        private void trackbar_MouseDown(object sender, MouseEventArgs e)
        {
            trackBasýlý = true;
        }

        private void ses_Click(object sender, EventArgs e)
        {
            if (outEvent != null)
            {
                if (outEvent.Volume != 0)
                {
                    outEvent.Volume = 0;
                    ses.Image = Resource1.Mute;
                    sesscroll.Value = 0;
                }
                else
                {
                    outEvent.Volume = 0.5f;
                    ses.Image = Resource1.Voicemax;
                    sesscroll.Value = 50;
                }
            }
            else
            {
                return;
            }
        }

        private void sesscroll_Scroll(object sender, ScrollEventArgs e)
        {
            sesvalue = sesscroll.Value;
            sesvalue = sesvalue / 100f;
            if (outEvent != null && Reader != null)
            {
                outEvent.Volume = sesvalue;
            }
            else
            {
                return;
            }
            if (sesvalue == 0)
            {
                ses.Image = Resource1.Mute;
            }
            else
            {
                ses.Image = Resource1.Voicemax;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            trackbar.Value = Convert.ToInt32(Reader.CurrentTime.TotalSeconds);
            float trackvalue = trackbar.Value;

            if (trackvalue == 0)
            {
                trackbar.Value++;
                return;
            }
            else if (trackvalue % 60 == 0)
            {
                if (trackvalue == Convert.ToInt32(toplamsüre))
                {
                    if (repeat.Checked)
                    {
                        loop();
                    }
                    else
                    {
                        randomseç();
                    }
                }
                else
                {
                    trackbar.Value++;
                    float floatDakika = trackvalue / 60;
                    þuan.Text = floatDakika + ":00";
                }
            }
            else if (trackvalue == Convert.ToInt32(toplamsüre))
            {
                if (repeat.Checked)
                {
                    loop();
                }
                else
                {
                    randomseç();
                }
            }
            else
            {
                trackbar.Value++;
                float floatDakika = trackvalue / 60;
                dakika = int.Parse(floatDakika.ToString().Remove(floatDakika.ToString().IndexOf(",")));
                saniye = Convert.ToInt32(trackvalue % 60);
                string saniye2 = saniye.ToString();
                if (saniye < 10)
                {
                    saniye2 = "0" + saniye.ToString();
                }
                þuan.Text = dakika.ToString() + ":" + saniye2;
            }
        }
    }
}
