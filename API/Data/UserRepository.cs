using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto?> GetMemberAsync(string username)
        {
            var res = await _context.Users!
                .Where(x => x.Username == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) // Такой маппинг позволяет не забирать из БД все поля для AppUser, а брать только то, что нужно в MemberDto. ConfigurationProvider инициализируется через DI в классе AutomapperProfiler
                .SingleOrDefaultAsync();

            return res;
        }

        public async Task<PagedList<MemberDto?>?> GetMembersAsync(UserParams userParams)
        {
            var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);

            if (_context.Users != null)
            {
                var query = _context.Users.AsQueryable()
                                          .Where(x => x.Username != userParams.CurrentUsername)
                                          .Where(x => x.Gender == userParams.Gender)
                                          .Where(x => x.DateOfBirth >= minDateOfBirth && x.DateOfBirth <= maxDateOfBirth)
                                          .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                          .AsNoTracking(); // Здесь нам нужно только читать из БД, поэтому трекинг не нужен ;

                return await PagedList<MemberDto?>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
            }
            return null;
        }

        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await _context.Users!.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users!
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<IEnumerable<AppUser?>> GetUsersAsync()
        {
            return await _context.Users!
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}