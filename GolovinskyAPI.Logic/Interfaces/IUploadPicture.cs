using GolovinskyAPI.Data.Models.Background;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace GolovinskyAPI.Logic.Interfaces
{
    public interface IUploadPicture
    {
        byte[] UplodaBase64Image(string uploadImage);

        string GetBase64Image(byte[] byteImage);

        Task<byte[]> UploadPicture(string appCode, IFormFile image);

        string GetImagePath(string appCode, string fileName);

        bool RemoveBackground(Background background);
    }
}
