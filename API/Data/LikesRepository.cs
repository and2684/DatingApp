using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public LikesRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        // sourceUserId, likeUserId - это составной ключ, по нему можно искать через FindAsync (чудо-то какое)
        public async Task<UserLike?> GetUserLike(int sourceUserId, int likeUserId)
        {
            return await _context.Likes!.FindAsync(sourceUserId, likeUserId);
        }

        // Список пользователей, которых лайкнул пользователь userId
        public async Task<IEnumerable<LikeDto>?> GetUserLikes(string predicate, int userId)
        {
            var users = _context.Users!.AsQueryable();
            var likes = _context.Likes!.AsQueryable();

            if (predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == userId);
                users = likes.Select(like => like.LikedUser)!;
            }

            if (predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == userId);
                users = likes.Select(like => like.SourceUser)!;
            }

            // return await users.Select(user => new LikeDto
            // {
            //     Username = user.Username,
            //     KnownAs = user.KnownAs,
            //     Age = user.DateOfBirth.CalculateAge(),
            //     PhotoUrl = user.Photos!.FirstOrDefault(p => p.IsMain)!.Url,
            //     City = user.City
            // }).ToListAsync();

            return await users.ProjectTo<LikeDto>(_mapper.ConfigurationProvider).ToListAsync(); // Автомаппером проще
        }

        // Получить юзера с его коллекцией лайков
        public async Task<AppUser?> GetUserWithLikes(int userId)
        {
            return await _context.Users!
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}