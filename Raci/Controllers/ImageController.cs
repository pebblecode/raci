namespace Raci.Controllers
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;

    using ImageProcessor;

    using ImageSource;

    public class ImageController : ApiController
    {
        public HttpResponseMessage Get(string tags)
        {
            var flickrService = new FlickrImageService();
            var images = flickrService.GetImages(tags).Take(1).ToArray();
            var processor = new Processor();
            var resultImage = processor.ProcessImage(images.First(), images, 20, 20);
            HttpResponseMessage result;
            using (var ms = new MemoryStream())
            {
                var bitmap = new Bitmap(resultImage);
                bitmap.Save(ms, ImageFormat.Jpeg);
                result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(ms.ToArray()) };
            }
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return result;
        }
    }
}
