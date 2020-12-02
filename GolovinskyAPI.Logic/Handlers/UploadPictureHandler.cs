using GolovinskyAPI.Data.Models.Background;
using GolovinskyAPI.Logic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace GolovinskyAPI.Logic.Handlers
{
    public class UploadPictureHandler : IUploadPicture
    {
        private readonly IHostEnvironment _env;

        public UploadPictureHandler(IHostEnvironment env)
        {
            _env = env;
        }

        public string GetBase64Image(byte[] byteImage)
        {
            string image = Convert.ToBase64String(byteImage);

            return image;
        }

        public byte[] UplodaBase64Image(string uploadImage)
        {
            byte[] imageBytes = Convert.FromBase64String(uploadImage);

            return imageBytes;
        }

        public async Task<byte[]> UploadPicture(string appCode, IFormFile image)
        {
            //var webRoot = _env.ContentRootPath;
            //var directory = Path.Combine(webRoot, "wwwroot", "images", "backgrounds", appCode);

            //if (!Directory.Exists(directory))
            //{
            //    DirectoryInfo dir = Directory.CreateDirectory(directory);
            //}

            //string path = Path.Combine(directory, image.FileName);

            //var stream = new FileStream(path, FileMode.Create);
            //stream.Position = 0;
            //await image.CopyToAsync(stream);
            //await stream.FlushAsync();
            //stream.Close();

            var bytes = GetImageBytesArray(image);
            return bytes;
        }


        private byte[] GetImageBytesArray(IFormFile uploadImage)
        {
            var image = Image.FromStream(uploadImage.OpenReadStream(), true, true);
            var s = new Size(image.Width, image.Height);
            var bmp = new Bitmap(image, s);
            byte[] fileBytes;

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Jpeg);
                fileBytes = ms.ToArray();
            }

            return fileBytes;
        }

        public string GetImagePath(string appCode, string fileName)
        {
            var webRoot = _env.ContentRootPath;
            var directory = Path.Combine(webRoot, "wwwroot", "images", "backgrounds", appCode);

            if (!Directory.Exists(directory))
            {
                DirectoryInfo dir = Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, fileName);

            return path;
        }

        public bool RemoveBackground(Background background)
        {
            var webRoot = _env.ContentRootPath;
            var path = Path.Combine(webRoot, "wwwroot", "images", "backgrounds", background.AppCode, background.FileName);

            if (!Directory.Exists(path))
            {
                return false;
            }

            else
            {
                Directory.Delete(path);
                return true;
            }
        }
    }
}
