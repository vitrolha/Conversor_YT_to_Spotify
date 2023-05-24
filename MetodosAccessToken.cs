using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    internal class MetodosAccessToken
    {
        HttpClient client = new HttpClient();
        /*private string jsonFilePath = "E:\\Projetos C#\\YoutubeDownloader_e_ConversorDePlaylist(v1.1)\\YoutubeDownloader_e_ConversorDePlaylist(v1.1)\\SpotifyAccessesTokens.json";*/
        //private string jsonFilePath = "";
        private string strJson = "";
        DateTime DataHora_token;
        private string Token;

        public string JsonFilePath()
        {
           return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\SpotifyAccessesTokens.json";      
        }

        private async Task<string> AccessToken()
        {
            try
            {
                client.BaseAddress = new Uri("https://accounts.spotify.com/api/token");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                request.Content = new StringContent($"grant_type=client_credentials&client_id=f8831e6503224e538df2b93ae3916bcd&client_secret=1acf40f7cc29447ba8b2405e5239bbda", Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await client.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();
                var accesstoken = JsonConvert.DeserializeObject<AccessesTokens>(responseContent);

                return accesstoken.access_token;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<bool> GravarAccessToken()
        {
            try
            {

                GettersSetters RegistrarToken = new GettersSetters()
                {
                    accessesTokens = new List<AccessesTokens>()
                {
                    new AccessesTokens()
                    {

                        access_token = await AccessToken(),
                        data_hora = DateTime.Now
                    }
                }
                };

                string json = JsonConvert.SerializeObject(RegistrarToken, Formatting.Indented);

                File.WriteAllText(JsonFilePath(), json);

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
                             
        }

        /// <summary>
        /// Verifica a validade do token, caso ele tenha sido criado a menos de 1 hora (uma hora) o metódo retornará true(token válido), porém se o token tenha sido criado a mais de 1 hora (uma hora) retornará false(token inválido)
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TokenValido()
        {
            try
            {
                using (StreamReader sr = new StreamReader(JsonFilePath()))
                {
                    strJson = sr.ReadToEnd();
                }
                var tokenInfos = JsonConvert.DeserializeObject<GettersSetters>(strJson);
                foreach (var infos in tokenInfos.accessesTokens)
                {
                    DataHora_token = infos.data_hora;
                }
                int validade = DateTime.Compare(DataHora_token.AddMinutes(60), DateTime.Now);
                if (validade < 0)
                {
                    //MessageBox.Show("Passou da validade");
                    return false;

                }
                else
                {
                    //MessageBox.Show("Token válido");
                    return true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string LerAccessToken()
        {
            try
            {
                using (StreamReader sr = new StreamReader(JsonFilePath()))
                {
                    strJson = sr.ReadToEnd();
                }
                var tokenInfos = JsonConvert.DeserializeObject<GettersSetters>(strJson);
                foreach (var infos in tokenInfos.accessesTokens)
                {
                    Token = infos.access_token;
                }
                return Token;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

    }

      
    
}
