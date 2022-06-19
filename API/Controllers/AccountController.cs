using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken.");

            using var hmac = new HMACSHA512(); // Хэш-алгоритм для шифрования пароля. Здесь генерируется ключ, с помощью которого хэш пароля будет преобразован в пароль (hmac.Key)

            var user = new AppUser()
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // Храним не сам пароль, а его хэш
                PasswordSalt = hmac.Key // И уникальный ключ, сгенерированный при создании объекта hmac
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Вместо AppUser теперь возвращаем UserDto, в конструкторе которого мы генерируем jwt-токен с помощью token service
            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Username == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username.");
            if (user.PasswordHash == null || user.PasswordSalt == null) return Unauthorized("Invalid password hash/salt");

            using var hmac = new HMACSHA512(user.PasswordSalt); // Теперь мы расшифровываем пароль с помощью Ключа, хранящегося в PasswordSalt

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));                       
            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password."); // Проверяем посимвольно хэш пароля
            }

            // Вместо AppUser теперь возвращаем UserDto, в конструкторе которого мы генерируем jwt-токен с помощью token service
            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {            
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }
    }
}