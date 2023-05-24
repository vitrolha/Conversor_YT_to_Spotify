using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.LabelEdit = true;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Columns.Add("#", 30, HorizontalAlignment.Left);
            listView1.Columns.Add("Artista", 200,HorizontalAlignment.Left);
            listView1.Columns.Add("Musicas", 500, HorizontalAlignment.Left);
        }

        MetodosAccessToken metodosAccessToken = new MetodosAccessToken();
        MetodosSpotify metodosSpotify = new MetodosSpotify();

        MetodosYoutube metodosYoutube = new MetodosYoutube();    
        private async void btn_Converter_Click(object sender, EventArgs e)
        {       
            string resultadoJson = await metodosSpotify.GetRequest(txtBox_linkSpotifyURL.Text);
            var nomeDasMusicas = metodosSpotify.NomeDasMusicas(resultadoJson);
            var ids = await metodosYoutube.Pesquisar_e_VideoID(nomeDasMusicas);

            if (txtBox_tituloPlaylist.Text == "") txtBox_LinkYTPlaylist.Text = "PlayList Conversor";

            var playlist = await metodosYoutube.CriarPlaylistYT(ids, txtBox_tituloPlaylist.Text);
            txtBox_LinkYTPlaylist.Text = "https://www.youtube.com/playlist?list=" + playlist;
        }

        private async void txtBox_linkSpotifyURL_TextChanged(object sender, EventArgs e)
        {            
            if (await metodosAccessToken.TokenValido())
            {
                //token válido
                //fazer as coisas
                string resultadoJson = await metodosSpotify.GetRequest(txtBox_linkSpotifyURL.Text);
                metodosSpotify.ExibirMusicas(listView1, resultadoJson);
            }
            else
            {
                //token invalido
                //gerar um novo
                try
                {
                    await metodosAccessToken.GravarAccessToken();
                }
                finally
                {
                    string resultadoJson = await metodosSpotify.GetRequest(txtBox_linkSpotifyURL.Text);
                    metodosSpotify.ExibirMusicas(listView1, resultadoJson);
                }
            }        
        }

        private async void botao_teste_Click(object sender, EventArgs e)
        {            
            string musicas = await metodosSpotify.GetRequest(txtBox_linkSpotifyURL.Text);

            //var lista = metodosSpotify.NomeDasMusicas(musicas);

            //MessageBox.Show(string.Join("\n", lista));
            var nomes = metodosSpotify.NomeDasMusicas(musicas);
            //o metodo pega o id um por um tenho que usar varias vezes
            
            var ids =await metodosYoutube.Pesquisar_e_VideoID(nomes);
                     
            //var PlaylistURL = await metodosYoutube.CriarPlaylistYT(ids);
            //MessageBox.Show(PlaylistURL);
        }


    }
}
