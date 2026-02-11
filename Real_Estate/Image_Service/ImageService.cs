using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Threading.Tasks;

namespace Real_Estate.Image_Service
{
    public record ImageUpload(string Url , string PublicId);
    public record CloudSetting(string CloudName, string ApiKey, string ApiSecret);
    public class ImageService
    {
        private readonly IConfiguration _configuration;
        private readonly CloudSetting _cloudSetting;
        private Cloudinary _cloudinary;
        public ImageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _cloudSetting = _configuration.GetSection("CloudinarySettings").Get<CloudSetting>()!;
            var account = new Account(_cloudSetting.CloudName, _cloudSetting.ApiKey, _cloudSetting.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<ImageUpload> ImageUploadResultAsync(IFormFile file , IFormFile[]? files = null , string? folder = null)
        {
            var uploadResult = new ImageUploadResult();
            if(file is not null && file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Folder = folder
                    };
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }
            return new ImageUpload(uploadResult.Url.ToString(),uploadResult.PublicId);
        }
        public async Task<List<ImageUpload>> ImageUploadResultAsync(IFormFile[] files, string? folder = null)
        {
            var uploadResult = new ImageUploadResult();
            List<ImageUpload> imageUploads = new List<ImageUpload>();   
            if (files is not null && files.Length > 0)
            {
                foreach(var file in files)
                {
                    using(var stream = file.OpenReadStream())
                    {
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.Name, stream),
                            Folder = folder
                        };
                        uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        imageUploads.Add(new ImageUpload(uploadResult.Url.ToString(), uploadResult.PublicId));
                    }
                }
            }
            return imageUploads;
        }
        public async Task<bool> DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var deleteResult = await _cloudinary.DestroyAsync(deletionParams);
            return deleteResult.Result == "ok";
        }

    }
}
