using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Videos;
using YoutubeExplode;
using System.Windows.Forms;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Playlists;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    internal class PlaylistDownload : Videos
    {
        private int musicasTotal;
        public int musicasRestantes;
        public PlaylistDownload(YoutubeClient youtube, Video video, string URL) : base(youtube, video, URL)
        {
            musicasTotal = 0;
            musicasRestantes = 0;
        }

        override public async void MostrarVideosNaLista(ListView lista)
        {
            try
            {
                var playlist = await youtube.Playlists.GetVideosAsync(URL);
                lista.Items.Clear();
                foreach (var video in playlist)
                {
                    lista.Items.Add(video.Title);
                }
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

                var playlist = await youtube.Playlists.GetVideosAsync(URL);
                foreach (var video in playlist)
                {
                    tituloDoVideo = padrao.Replace(video.Title, " ");
                    var url_video = video.Url;
                    await youtube.Videos.DownloadAsync(url_video, $"{path}" + "\\" + $"{tituloDoVideo}.mp3", progress);
                }
                MessageBox.Show("Vídeos baixado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
