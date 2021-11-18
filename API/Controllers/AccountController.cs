using System.Security.Cryptography;
using System.Net.Cache;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using API.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController: BaseController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)  
        {
            if(await UserExist(dto.Username.ToLower())) return BadRequest("Username is taken");
            
            using var hmac = new HMACSHA512();

            var user = new AppUser{
                UserName = dto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto {
                UserName = user.UserName,
                Token = _tokenService.GetToken(user)
            };
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)  
        {
            var user = await _context.Users.SingleOrDefaultAsync( x=> x.UserName == dto.Username.ToLower());

            if(user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            
            return new UserDto {
                UserName = user.UserName,
                Token = _tokenService.GetToken(user)
            };
        }

        private async Task<bool> UserExist(string username) => await _context.Users.AnyAsync(x=> x.UserName == username.ToLower());

    }
}