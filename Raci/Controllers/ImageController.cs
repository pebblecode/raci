namespace Raci.Controllers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;

    using ImageSource;

    public class ImageController : ApiController
    {
        public object Get(string tags)
        {
            try
            {
                var flickrService = new FlickrImageService();
                var image = flickrService.GetImages(tags).First();
                HttpResponseMessage result;
                using (var ms = new MemoryStream())
                {
                    var bitmap = new Bitmap(image);
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(ms.ToArray()) };
                }
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                return result;
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}
