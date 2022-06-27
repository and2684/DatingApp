using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, 
                               IPhotoService photoService)
        {
            _photoService = photoService;
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
        [HttpGet("{username}", 
                 Name = "GetUser")] // Добавил Name, чтобы использовать его в ответе 201 CreatedAtRoute в методе AddPhoto (ниже) // Add MAS 22.06.2022
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
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _userRepository.Update(user!);            

            if (await _userRepository.SaveAllAsync()) 
                return NoContent();
            else 
                return BadRequest("Fail update user");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user?.Photos?.FirstOrDefault(x => x.Id == photoId);

            if (photo is null) return NotFound("No photo is chosen");
            if (photo.IsMain) return BadRequest("This is already a main photo");

            var currentMain = user?.Photos?.FirstOrDefault(x => x.IsMain);
            if (currentMain is not null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Fail to set main photo");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error != null)
                return BadRequest(result.Error.Message);
            
            var photo = new Photo // Добавленная фотка
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = (user?.Photos?.Count == 0) // Если у пользователя не было фоток пока мы не сделали AddPhoto, то установить фотку как аватарку
            };

            user!.Photos!.Add(photo);
            
            if (await _userRepository.SaveAllAsync())
            {
                //return _mapper.Map<PhotoDto>(photo); // Это работало, но возвращало 200 ОК, а при добавлении фотки правильно возвращать 201 Created
                return CreatedAtRoute("GetUser", new{username = user.Username}, _mapper.Map<PhotoDto>(photo)); // Другое дело, тут вернется 201. В качестве места, где можно взять новый ресурс (фотку) будет указан метод GetUser с параметром "{username}" = наш user.Username, а сама фотография передана в photoDTO
            }
            else
                return BadRequest("Problem adding photo");
        }        

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user?.Photos?.FirstOrDefault(x => x.Id == photoId);

            if (photo is null) return NotFound("No photo is chosen");
            if (photo.IsMain) return BadRequest("Cannot delete main photo");  
                      
            if (photo.PublicId is not null) 
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }
            else 
                return BadRequest("Incorrect photo id");
                
            user?.Photos?.Remove(photo);
            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete photo");
        }

    }
}