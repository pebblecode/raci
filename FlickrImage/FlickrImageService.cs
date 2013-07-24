namespace ImageSource
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;

    using FlickrNet;

    public class FlickrImageService
    {
        public IEnumerable<FlickrImage> GetImages(string tags)
        {
            Flickr.CacheDisabled = true;
            var flickr = new Flickr("340b341adedd9b2613d5c447c4541e0f");
            flickr.InstanceCacheDisabled = true;
            var options = new PhotoSearchOptions { Tags = tags, PerPage = 2  };
            var photos = flickr.PhotosSearch(options);
            return photos.Select(i =>
            {
                using (var client = new WebClient())
                {
                    var data = client.DownloadData(i.Medium640Url);
                    using (var memoryStream = new MemoryStream(data))
                    {
                        var bitmap = new Bitmap(memoryStream);
                        return new FlickrImage
                                   {
                                       Url = i.Medium640Url,
                                       Image = new Bitmap(bitmap),
                                       Encoded = Convert.ToBase64String(memoryStream.ToArray())
                                   };
                    }
                }
            });
        }
    }
    public class FlickrImage
    {
        public string Url { get; set; }
        public Bitmap Image { get; set; }
        public string Encoded { get; set; }
    }
}
