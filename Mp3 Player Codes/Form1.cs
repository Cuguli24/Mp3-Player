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

        bool �al���yor = false;
        bool trackBas�l� = false;

        int se�ili�ndex;
        int totalMusics;

        float sesvalue = 0.5f;

        double toplams�re;

        int dakika;
        int saniye;


        public Form1()
        {
            InitializeComponent();
        }


        private void �gese�()
        {
            string se�ilen�ge = musics[se�ili�ndex].ToString();
            isim.Text = se�ilen�ge;
            se�ilen�ge = $"C:\\Users\\{ComputerName}\\Music" + "\\" + se�ilen�ge;
            outEvent = new WaveOutEvent();
            Reader = new AudioFileReader(se�ilen�ge);
            outEvent.Init(Reader);
            toplams�re = Reader.TotalTime.TotalSeconds;
            trackbar.Maximum = Convert.ToInt32(toplams�re);
            outEvent.Play();
            timer.Start();
            double floatDakika = toplams�re / 60;
            dakika = int.Parse(floatDakika.ToString().Remove(floatDakika.ToString().IndexOf(",")));
            saniye = Convert.ToInt32(toplams�re % 60);
            if (saniye < 10)
            {
                s�re.Text = dakika.ToString() + ":0" + saniye.ToString();
            }
            else
            {
                s�re.Text = dakika.ToString() + ":" + saniye.ToString();
            }
            trackbar.Value = 0;
            �uan.Text = "0:00";
            guna2PictureBox1.Image = Resource1.Pause;
            Logo.Image = Resource1.musicgif;
            �al���yor = true;
        }

        private void durdur()
        {
            outEvent.Pause();
            guna2PictureBox1.Image = Resource1.Play;
            Logo.Image = Resource1.CUGULI__1_;
            �al���yor = false;
            timer.Stop();
        }

        private void �al��t�r()
        {
            outEvent.Play();
            guna2PictureBox1.Image = Resource1.Pause;
            Logo.Image = Resource1.musicgif;
            �al���yor = true;
            timer.Start();
        }

        private void randomse�()
        {
            Random random = new Random();
            int randomSay� = random.Next(0, totalMusics);
            while (randomSay� == se�ili�ndex)
            {
                randomSay� = random.Next(0, totalMusics);
            }
            outEvent.Dispose();
            Reader.Dispose();
            se�ili�ndex = randomSay�;
            �gese�();
            �al��t�r();
        }

        private void loop()
        {
            outEvent.Dispose();
            Reader.Dispose();
            �gese�();
            �al��t�r();
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
                    M�zikler.Items.Add(musics[i]);
                }
            }
        }

        private void addmusic_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Cuguli M�zik | Select a MP3 file";
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

                    M�zikler.Items.Add(musicName);
                }
                else
                {
                    MessageBox.Show("This File Already Exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                musics = File.ReadAllLines(content);
            }
        }

        private void M�zikler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (outEvent != null && Reader != null)
            {
                outEvent.Dispose();
                Reader.Dispose();
                guna2PictureBox1.Image = Resource1.Play;
                Logo.Image = Resource1.CUGULI__1_;
                �al���yor = false;

            }
            if (M�zikler.SelectedItem != null)
            {
                se�ili�ndex = M�zikler.SelectedIndex;
                �gese�();
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if (outEvent != null && !�al���yor)
            {
                �al��t�r();
            }

            else if (outEvent != null && �al���yor)
            {
                durdur();
            }
        }

        private void �nceki_Click(object sender, EventArgs e)
        {
            timer.Stop();
            trackbar.Value = 0;
            �uan.Text = "0:00";
            if (outEvent != null && Reader != null)
            {
                if (Reader.CurrentTime.TotalSeconds <= 5)
                {
                    outEvent.Dispose();
                    Reader.Dispose();
                    guna2PictureBox1.Image = Resource1.Play;
                    Logo.Image = Resource1.CUGULI__1_;
                    �al���yor = false;


                    if (se�ili�ndex > 0)
                    {
                        se�ili�ndex--;
                        �gese�();

                    }
                    else
                    {
                        se�ili�ndex = musics.Length - 1;
                        �gese�();
                    }
                }

                else
                {
                    timer.Stop();
                    trackbar.Value = 0;
                    Reader.CurrentTime = TimeSpan.FromSeconds(trackbar.Value);
                    �uan.Text = "0:00";
                    timer.Start();
                }
            }
        }

        private void sonraki_Click(object sender, EventArgs e)
        {
            timer.Stop();
            trackbar.Value = 0;
            �uan.Text = "0:00";
            if (outEvent != null && Reader != null)
            {
                outEvent.Dispose();
                Reader.Dispose();
                guna2PictureBox1.Image = Resource1.Play;
                Logo.Image = Resource1.CUGULI__1_;
                �al���yor = false;


                if (se�ili�ndex < totalMusics - 1)
                {
                    se�ili�ndex++;
                    �gese�();

                }
                else
                {
                    se�ili�ndex = 0;
                    �gese�();
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
                �al���yor = false;
                timer.Stop();

                float trackvalue = trackbar.Value;
                if (trackvalue == 0)
                {
                    return;
                }
                else if (trackvalue % 60 == 0)
                {
                    float floatDakika = trackvalue / 60;
                    �uan.Text = dakika + ":00";
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
                    �uan.Text = dakika.ToString() + ":" + saniye2;
                }
            }
        }

        private void trackbar_MouseUp(object sender, MouseEventArgs e)
        {
            if (outEvent != null && e.Button==MouseButtons.Left)
            {
                trackBas�l� = false;
                string se�ilen�ge = musics[se�ili�ndex].ToString();
                isim.Text = se�ilen�ge;
                se�ilen�ge = $"C:\\Users\\{ComputerName}\\Music" + "\\" + se�ilen�ge;
                outEvent = new WaveOutEvent();
                Reader = new AudioFileReader(se�ilen�ge);
                outEvent.Init(Reader);
                Reader.CurrentTime = TimeSpan.FromSeconds(trackbar.Value);
                outEvent.Play();
                guna2PictureBox1.Image = Resource1.Pause;
                Logo.Image = Resource1.musicgif;
                �al���yor = true;
                timer.Start();
                if (trackbar.Value == Convert.ToInt32(toplams�re))
                {
                    if (repeat.Checked)
                    {
                        loop();
                    }
                    else
                    {
                        randomse�();
                    }
                }
            }
        }

        private void trackbar_MouseDown(object sender, MouseEventArgs e)
        {
            trackBas�l� = true;
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
                if (trackvalue == Convert.ToInt32(toplams�re))
                {
                    if (repeat.Checked)
                    {
                        loop();
                    }
                    else
                    {
                        randomse�();
                    }
                }
                else
                {
                    trackbar.Value++;
                    float floatDakika = trackvalue / 60;
                    �uan.Text = floatDakika + ":00";
                }
            }
            else if (trackvalue == Convert.ToInt32(toplams�re))
            {
                if (repeat.Checked)
                {
                    loop();
                }
                else
                {
                    randomse�();
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
                �uan.Text = dakika.ToString() + ":" + saniye2;
            }
        }
    }
}
