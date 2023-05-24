using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    internal class MetodosSpotify
    {
        MetodosAccessToken MetodosAT = new MetodosAccessToken();
        private string playlistID = @"playlist\/([\w\W]+)";
        private string ID;
        private string playlistURL = "https://api.spotify.com/v1/playlists/";
        HttpClient client = new HttpClient();
        private string URLAPI;
        private string AccessToken;
        private int nmro_musica;
        private string name_artist;
        

        public async Task<string> GetRequest(string PlaylistURL)
        {
            try
            {
                AccessToken = MetodosAT.LerAccessToken();

                Match match = Regex.Match(PlaylistURL, playlistID);
                ID = match.Groups[1].Value;

                URLAPI = playlistURL + ID;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

                HttpResponseMessage request = await client.GetAsync(URLAPI);

                HttpContent conteudo = request.Content;

                string resposta = await conteudo.ReadAsStringAsync();

                return resposta;

                /* var json = JsonConvert.DeserializeObject<GettersSetters>(resposta);

                //exibe na lista as musicas
                lista.Items.Clear();
                nmro_musica = 1;

                foreach(var item in json.tracks.items)
                {
                    lista.Items.Add(new ListViewItem(new[] { nmro_musica.ToString(), item.track.name }));
                    nmro_musica++;
                }*/
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public void ExibirMusicas(ListView lista, string respostaJson)
        {
            try
            {
                var json = JsonConvert.DeserializeObject<GettersSetters>(respostaJson);
                //exibe na lista as musicas
                lista.Items.Clear();
                nmro_musica = 1;
                if (json != null)
                {
                    foreach (var item in json.tracks.items)
                    {
                        foreach (var name in item.track.artists)
                        {
                            name_artist = name.name;
                        }
                        lista.Items.Add(new ListViewItem(new[] { nmro_musica.ToString(), name_artist, item.track.name }));
                        nmro_musica++;
                    }
                }
                else
                {
                    MessageBox.Show("URL Inválida","URL inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message, "URL inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }   
        
       public List<string> NomeDasMusicas(string respostaJson)
       {
            List<string> Nome_Musicas = new List<string>();
            try
            {
                var json = JsonConvert.DeserializeObject<GettersSetters>(respostaJson);
                if(json != null)
                {
                    foreach(var item in json.tracks.items)
                    {
                        foreach(var name in item.track.artists)
                        {
                            name_artist= name.name;
                        }
                        Nome_Musicas.Add(name_artist + " " + item.track.name);                        
                    }
                    
                }
                return Nome_Musicas;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
       }
    }
}
