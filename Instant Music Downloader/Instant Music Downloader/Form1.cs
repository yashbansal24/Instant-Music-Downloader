using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace Instant_Music_Downloader
{
    public partial class Form1 : Form
    {
        static int count = 0;
        WebClient client, webclient;
        String not_done,dest,global_song;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "Starting Download ... Please Wait ...";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dest = fbd.SelectedPath;
            }
            int x = 0;
            string[] sep = new string[] { "\r\n" };
            string[] lines = textBox1.Text.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                x = download(lines[i]);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private int download(string song)
        {
            
            string name = song;
            global_song = name;
            song = query(song);
            using (webclient = new WebClient())
            {
                string reply = webclient.DownloadString("https://www.youtube.com/results?search_query=" + song);
                string download_url = Embed(reply);
                using (client = new WebClient())
                {
                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                    client.DownloadFileAsync(new Uri("https://www.youtubeinmp3.com/fetch/?video=" + download_url), @dest + "\\" + name + ".mp3");
                }
            }

            return 0;



        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            count++;
            label1.Text = "Downloaded " + count.ToString() + " song(s)";
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage <= 1 && e.TotalBytesToReceive<=1024*1024)
            {
                e.ProgressPercenge = 100;
                count--;
                not_done[not_done.Length] = global_song;
            }
            
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = "Downloading";
        }
        private string query(string s)
        {
            string[] words = s.Split(' ');
            string re = "";
            foreach (string word in words)
            {
                re += word + "+";
            }
            return re.Substring(0, re.Length - 1);
        }

        private String Embed(string s)
        {
            int n = s.Length;
            int index = s.IndexOf("/watch?v=");
            index = index + "/watch?v=".Length;
            string re = "https://www.youtube.com/watch?v=";

            for (int i = index; i < n; i++)
            {
                if (s[i] == '"') break;

                else re += "" + s[i];
            }

            return re;

        }

        

        
    }
}
