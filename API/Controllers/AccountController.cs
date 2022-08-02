using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken.");

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512(); // Хэш-алгоритм для шифрования пароля. Здесь генерируется ключ, с помощью которого хэш пароля будет преобразован в пароль (hmac.Key)

            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)); // Храним не сам пароль, а его хэш
            user.PasswordSalt = hmac.Key; // И уникальный ключ, сгенерированный при создании объекта hmac

            _context.Users!.Add(user);
            await _context.SaveChangesAsync();

            // Вместо AppUser теперь возвращаем UserDto, в конструкторе которого мы генерируем jwt-токен с помощью token service
            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users!
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Username == loginDto.Username);

            if (user == null) return Unauthorized("Invalid username.");
            if (user.PasswordHash == null || user.PasswordSalt == null) return Unauthorized("Invalid password hash/salt");
            if (user.Username == null) return Unauthorized("Username is empty");

            using var hmac = new HMACSHA512(user.PasswordSalt); // Теперь мы расшифровываем пароль с помощью Ключа, хранящегося в PasswordSalt

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password!));                       
            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password."); // Проверяем посимвольно хэш пароля
            }

            // Вместо AppUser теперь возвращаем UserDto, в конструкторе которого мы генерируем jwt-токен с помощью token service
            return new UserDto
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string username)
        {            
            return await _context.Users!.AnyAsync(x => x.Username == username.ToLower());
        }
    }
}