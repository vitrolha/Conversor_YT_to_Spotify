using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;


namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    internal class MetodosYoutube
    {
        private SearchListResponse searchListResponse;
        private UserCredential credential;
        public async Task<List<string>> Pesquisar_e_VideoID(List<string> nomes_musicas)
        {
            try
            {
                List<string> videoID = new List<string>();

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyBK6FD7bMqj1JSduwRsE8RFB_-4mlZVy0E",
                    ApplicationName = "Conversor"
                });

                foreach (var nome_musica in nomes_musicas)
                {
                    var searchListRequest = youtubeService.Search.List("snippet");

                    searchListRequest.Q = nome_musica;
                    searchListRequest.MaxResults = 1;
                    searchListResponse = await searchListRequest.ExecuteAsync();
                    //searchList = (100*quantidade total de musicas)

                    foreach (var searchResult in searchListResponse.Items)
                    {
                        if (searchResult.Id.Kind == "youtube#video") videoID.Add(searchResult.Id.VideoId);
                    }
                }

                return videoID;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        //Tenho um total de 100 queries por dia, sendo que cada requisição de pesquisa por ID usa 1
        //ou seja, posso converter um total de 99 musicas por dia para nao passar do limite
        
        //Fazer o metodo de criar a playslist com os ids coletados

        public async Task<string> CriarPlaylistYT(List<string>videosID, string tituloPlaylist)
        {
            try
            {


                //vou precisar autenticar com Oauth2
                using (var stream = new FileStream(@"E:\Projetos C#\YoutubeDownloader_e_ConversorDePlaylist(v1.1)\YoutubeDownloader_e_ConversorDePlaylist(v1.1)\Client_Secret_Youtube.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,

                        new[] { YouTubeService.Scope.Youtube },
                        "user",
                        CancellationToken.None,
                        new FileDataStore(this.GetType().ToString())
                        );
                }

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Conversor"
                });

                var NEWPLAYLIST = new Playlist();
                NEWPLAYLIST.Snippet = new PlaylistSnippet();
                NEWPLAYLIST.Snippet.Title = tituloPlaylist;
                NEWPLAYLIST.Snippet.Description = "Playlist Criada Pelo App do Ongas";
                NEWPLAYLIST.Status = new PlaylistStatus();
                NEWPLAYLIST.Status.PrivacyStatus = "public";
                NEWPLAYLIST = await youtubeService.Playlists.Insert(NEWPLAYLIST, "snippet,status").ExecuteAsync();
                //playlistInsert = 50 para criar a playlist

                foreach (var videoID in videosID)
                {
                    var NEWPLAYLISTITEM = new PlaylistItem();
                    NEWPLAYLISTITEM.Snippet = new PlaylistItemSnippet();
                    NEWPLAYLISTITEM.Snippet.PlaylistId = NEWPLAYLIST.Id;
                    NEWPLAYLISTITEM.Snippet.ResourceId = new ResourceId();
                    NEWPLAYLISTITEM.Snippet.ResourceId.Kind = "youtube#video";
                    NEWPLAYLISTITEM.Snippet.ResourceId.VideoId = videoID;
                    NEWPLAYLISTITEM = await youtubeService.PlaylistItems.Insert(NEWPLAYLISTITEM, "snippet").ExecuteAsync();
                    //playlistInsertItem = (50*quantidade de videos pra colocar na playlist)
                }
                return NEWPLAYLIST.Id.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
