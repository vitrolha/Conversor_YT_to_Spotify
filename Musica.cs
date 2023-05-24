using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Videos;
using YoutubeExplode;
using System.Windows.Forms;
using YoutubeExplode.Converter;
using YoutubeExplode.Common;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    internal class Musica : Videos
    {
        public Musica(YoutubeClient youtube, Video video, string URL) : base(youtube, video, URL)
        {

        }
        override public async void MostrarVideosNaLista(ListView lista)
        {
            try
            {
                video = await youtube.Videos.GetAsync(URL);
                lista.Items.Clear();
                lista.Items.Add(video.Title);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "URL Invalida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        override public async Task<bool> BaixarMusicas(string path, ProgressBar progressBar)
        {
            try
            {
                var progress = new Progress<double>(value =>
                {
                    progressBar.Value = (int)(value * 100);
                });

                video = await youtube.Videos.GetAsync(URL);
                tituloDoVideo = padrao.Replace(video.Title, " ");
                await youtube.Videos.DownloadAsync(URL, $"{path}" + "\\" + $"{tituloDoVideo}.mp3", progress);
                MessageBox.Show("Vídeo baixado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
