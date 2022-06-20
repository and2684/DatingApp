using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        // api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers ()
        {
            // Del MAS 13.06.2022
            // Возвращаем сразу Ienumerable of MemberDto с помощью нового метода GetMembersAsync
            // var users = await _userRepository.GetUsersAsync();
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users); // Маппим users (ienumerable of users) в ienumerable of memberdto's
            // return Ok(usersToReturn);
            // End Del MAS 13.06.2022

            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

       
        // api/users/5
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto?>> GetUser (string username)
        {
            // Del MAS 13.06.2022
            // Возвращаем сразу MemberDto с помощью нового метода GetMemberAsync
            //var user = await _userRepository.GetUserByUsernameAsync(username);            
            //return _mapper.Map<MemberDto>(user);
            // End Del MAS 13.06.2022

            return await _userRepository.GetMemberAsync(username);
        }        

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user);            

            if (await _userRepository.SaveAllAsync()) 
                return NoContent();
            else 
                return BadRequest("Fail update user");
        }
    }
}