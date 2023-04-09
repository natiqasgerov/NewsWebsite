using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace NewsWebsite.Services
{
    public class MyCloud
    {
        private readonly Account account;
        private readonly Cloudinary cloudinary;
        public MyCloud()
        {
            account = new Account(CloudData.CloudName,CloudData.ApiKey,CloudData.ApiSecret);
            cloudinary = new Cloudinary(account);
        }

        public string UploadImage(byte[] bytes,string filename)
        {
            Stream stream = new MemoryStream(bytes);

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filename, stream),
                Folder = "NewsProjectMVC"
            };

            var uploadResult = cloudinary.Upload(uploadParams);

            cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(100).Height(150).Crop("fill"));

            return uploadResult.Url.ToString();
        }

    }
}
