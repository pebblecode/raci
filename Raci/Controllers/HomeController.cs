namespace Raci.Controllers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Mvc;

    using ImageProcessor;

    using ImageSource;

    public class HomeController : Controller
    {
        public ActionResult Index(string tags, string original, string block, int blockSize = 20)
        {
            var processor = new Processor();
            Bitmap resultImage;

            if (string.IsNullOrWhiteSpace(original) || string.IsNullOrWhiteSpace(block))
            {
                tags = tags ?? "cheese,stuff";
                var flickrService = new FlickrImageService();
                var images = flickrService.GetImages(tags).Take(2).ToArray();

                resultImage = processor.ProcessImage(
                    images.First().Image, images.Skip(1).Select(i => i.Image), blockSize, blockSize);
                HttpResponseMessage result;
                using (var ms = new MemoryStream())
                {
                    var bitmap = new Bitmap(resultImage);
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    return
                        View(
                            new FlickModel(
                                images.First().Encoded,
                                images.Skip(1).First().Encoded,
                                Convert.ToBase64String(ms.ToArray())));
                }
            }
            var originalImage = this.Load(original);
            var blockImage = this.Load(block);
            resultImage = processor.ProcessImage(originalImage.Image, new[] { blockImage.Image, }, blockSize, blockSize);

            using (var ms = new MemoryStream())
            {
                var bitmap = new Bitmap(resultImage);
                bitmap.Save(ms, ImageFormat.Jpeg);
                return View(new FlickModel(originalImage.Encoded, blockImage.Encoded, Convert.ToBase64String(ms.ToArray())));
            }
        }

        public FlickrImage Load(string url)
        {
            using (var client = new WebClient())
            {
                var data = client.DownloadData(url);
                using (var memoryStream = new MemoryStream(data))
                {
                    var bitmap = new Bitmap(memoryStream);
                    return new FlickrImage
                    {
                        Url = url,
                        Image = new Bitmap(bitmap),
                        Encoded = Convert.ToBase64String(memoryStream.ToArray())
                    };
                }
            }
        }
    }

    public class FlickModel
    {
        public string BaseEncoded { get; set; }

        public string BlockEncoded { get; set; }

        public string FinishedEncoded { get; set; }

        public FlickModel(string baseUrl, string blockUrl, string finishedUrl)
        {
            BaseEncoded = baseUrl;
            BlockEncoded = blockUrl;
            FinishedEncoded = finishedUrl;
        }
    }
}
