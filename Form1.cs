using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode.Videos;
using YoutubeExplode;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    public partial class Form1 : Form
    {
        YoutubeClient youtube = new YoutubeClient();
        Video video;
        private Regex videoLink = new Regex("https://www.youtube.com/watch");
        private Regex playlistLink = new Regex("https://www.youtube.com/playlist");

        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Add("Videos", 800, HorizontalAlignment.Left);
        }

        private void txtBox_YoutubeURL_TextChanged(object sender, EventArgs e)
        {
            if (videoLink.IsMatch(txtBox_YoutubeURL.Text))
            {
                Musica musica = new Musica(youtube, video, txtBox_YoutubeURL.Text);
                musica.MostrarVideosNaLista(listView1);
            }
            if (playlistLink.IsMatch(txtBox_YoutubeURL.Text))
            {
                PlaylistDownload playlist = new PlaylistDownload(youtube, video, txtBox_YoutubeURL.Text);
                playlist.MostrarVideosNaLista(listView1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.Show();
        }
    }
}
