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
            services.AddDbContext<DataContext>(async options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            
            // Сервис для создания токенов юзеров при регистрации/логине    
            services.AddScoped<ITokenService, TokenService>(); 

            // UserRepo
            services.AddScoped<IUserRepository, UserRepository>();     

            // Automapper
            services.AddAutoMapper(typeof(AutomapperProfiles).Assembly);

            return services;
        }        
    }
}