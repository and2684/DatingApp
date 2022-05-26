using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers ()
        {
            return await _context.Users.ToListAsync();
        }

        // api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser (int id)
        {
            return await _context.Users.FindAsync(id);
        }        
    }
}