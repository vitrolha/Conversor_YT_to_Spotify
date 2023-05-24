using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Videos;
using YoutubeExplode;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    internal abstract class Videos
    {
        protected Video video;
        protected string URL;
        protected YoutubeClient youtube = new YoutubeClient();
        protected string tituloDoVideo;
        protected Regex padrao = new Regex("[;,?*\\'\"| ]");
        public Videos(YoutubeClient youtube, Video video, string URL)
        {
            this.URL = URL;
            this.video = video;
            this.youtube = youtube;
        }
        public abstract void MostrarVideosNaLista(ListView lista);

        public abstract Task<bool> BaixarMusicas(string path, ProgressBar progressBar);


        public string SelecionarPath()
        {
            FolderBrowserDialog pasta = new FolderBrowserDialog();
            DialogResult result = pasta.ShowDialog();
            if (result == DialogResult.OK)
            {
                return pasta.SelectedPath;
            }
            return null;
        }

    }
}
