using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // EF core
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            
            // Сервис для создания токенов юзеров при регистрации/логине    
            services.AddScoped<ITokenService, TokenService>(); 

            // UserRepo
            services.AddScoped<IUserRepository, UserRepository>();     

            // Automapper
            services.AddAutoMapper(typeof(AutomapperProfiles).Assembly);

            // Настройки cloudinary, читаем их из appsettings.json
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));    

            // Сервис для добавления / удаления фоток в Cloudinary
            services.AddScoped<IPhotoService, PhotoService>();        

            return services;
        }        
    }
}