namespace ImageSource
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;

    using FlickrNet;

    public class FlickrImageService
    {
        public IEnumerable<Bitmap> GetImages(string tags)
        {
            Flickr.CacheDisabled = true;
            var flickr = new Flickr("340b341adedd9b2613d5c447c4541e0f");
            var options = new PhotoSearchOptions { Tags = tags, PerPage = 1  };
            var photos = flickr.PhotosSearch(options);
            return photos.AsParallel().Select(i =>
            {
                using (var client = new WebClient())
                {
                    var data = client.DownloadData(i.Medium640Url);
                    using (var memoryStream = new MemoryStream(data))
                    {
                        return new Bitmap(memoryStream);
                    }
                }
            });
        }
    }
}
