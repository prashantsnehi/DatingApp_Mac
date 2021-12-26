using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Extensions;
using API.Helpers;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        // private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork,
                               IMapper mapper,
                               IPhotoService photoService)
        {
            // _userRepository = userRepository;
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)  
        {
            // var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var gender = await _unitOfWork.UserRepository.GetUserGender(User.GetUsername());

            userParams.CurrentUserName = User.GetUsername();
            if (string.IsNullOrEmpty(userParams.Gender)) 
                userParams.Gender = gender == "male" ? "female" : "male";

            var users = await _unitOfWork.UserRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);
        }

        // [HttpGet("{id}")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public async Task<ActionResult<MemberDto>> GetUserById(int id) => Ok(await _userRepository.GetUserByIdAsync(id));

        [HttpGet("{username}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<MemberDto>> GetUser(string username) => 
                Ok(await _unitOfWork.UserRepository.GetMemberAsync(username));

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateMember([FromBody] MemberUpdateDto memberUpdateDto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Updation Failed");
        }

        [HttpPost("add-photo")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);
            if (await _unitOfWork.Complete()) 
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));

            return BadRequest("Problem adding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> SetMainPhoto(int photoId) 
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentName = user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentName != null) currentName.IsMain = false;

            photo.IsMain = true;

            if(await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePhoto(int photoId) 
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if(photo == null) return NotFound();

            if(photo.IsMain) return BadRequest("You can't delete the main photo");

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            } 

            user.Photos.Remove(photo);
            if(await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete photo");
        }
    }
}