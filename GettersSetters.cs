using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader_e_ConversorDePlaylist_v1._1_
{
    internal class GettersSetters
    {
        // public string access_token { get; set; }
        public List<AccessesTokens> accessesTokens { get; set; }

        public int total { get; set; }
        
        public Tracks tracks { get; set; }

        public QueriesPorDia queries { get; set; }
    }
    public class AccessesTokens
    {
        public string access_token { get; set; }
        public DateTime data_hora { get; set; }
        //public DateTime hora { get; set; }
    }

    public class Tracks
    {
        public List<Items> items { get; set; }
    }

    public class Items
    {
        public Track track { get; set; }
    }

    public class Track
    {
        public string name { get; set; }
        public List<Artists> artists { get; set; }
    }

    public class Artists
    {
        public string name { get; set; }
    }

    //Youtube
    public class QueriesPorDia
    {
        public int quantidade_queries { get; set; }
        public DateTime data_hora { get; set; }
    }
}
