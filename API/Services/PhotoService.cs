using API.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using API.Helpers;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary? _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            var _cloudinary = new Cloudinary(acc); // Инициализируем объект класса Cloudinary, который будет заниматься загрузкой и удалением фоток
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                if (_cloudinary != null)
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);               
            }

            return uploadResult;            
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);

            
            var result = new DeletionResult(); 
            
            if (_cloudinary != null)
                result = await _cloudinary.DestroyAsync(deletionParams);

            return result;
        }
    }
}