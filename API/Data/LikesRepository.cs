using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using API.Helpers;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        public LikesRepository(UserManager<AppUser> usermanager, DataContext context)
        {
            _userManager = usermanager;
            _context = context;
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId) => 
                    await _context.Likes.FindAsync(sourceUserId, likedUserId);

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _userManager.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }

            if(likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto {
                            Username = user.UserName,
                            KnownAs = user.KnownAs,
                            Age = user.DateOfBirth.CalculateAge(),
                            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                            City = user.City,
                            Id = user.Id
                        });
            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId) => 
                await _userManager.Users.Include(x => x.LikedUsers)
                                        .FirstOrDefaultAsync(x => x.Id == userId);
    }
}