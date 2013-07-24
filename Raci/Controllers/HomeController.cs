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
        public ActionResult Index(string tags)
        {
            tags = tags ?? "cheese,stuff";
            var flickrService = new FlickrImageService();
            var images = flickrService.GetImages(tags).Take(2).ToArray();

            var processor = new Processor();
            var resultImage = processor.ProcessImage(images.First().Image, images.Skip(1).Select(i => i.Image), 20, 20);
            HttpResponseMessage result;
            using (var ms = new MemoryStream())
            {
                var bitmap = new Bitmap(resultImage);
                bitmap.Save(ms, ImageFormat.Jpeg);
                return View(new FlickModel(images.First().Encoded, images.Skip(1).First().Encoded, Convert.ToBase64String(ms.ToArray())));
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
