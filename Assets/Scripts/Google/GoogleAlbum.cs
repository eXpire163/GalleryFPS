using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Google
{

    public class clsResponseAlbum
    {
        public List<Album> albums { get; set; }
        public string nextPageToken { get; set; }
    }
    public class Album
    {
        public string id { get; set; }
        public string title { get; set; }
        public string productUrl { get; set; }
        public int mediaItemsCount { get; set; }
        public string coverPhotoBaseUrl { get; set; }
        public string coverPhotoMediaItemId { get; set; }

    }
}
