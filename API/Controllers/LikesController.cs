using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class LikesController : BaseController
{
    // private readonly IUserRepository _unitOfWork.UserRepository;
    // private readonly ILikesRepository _unitOfWork.LikesRepository;
    private readonly IUnitOfWork _unitOfWork;
    // public LikesController(IUserRepository userRepository, ILikesRepository likesRepository)
    public LikesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        // _unitOfWork.UserRepository = userRepository;
        // _unitOfWork.LikesRepository = likesRepository;
        _unitOfWork = unitOfWork;

    }

    [Authorize]
    [HttpPost("{username}")]
    public async Task<ActionResult> AddLikes(string username)
    {
        var sourceUserId = User.GetUserId();
        // var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);

        if (likedUser == null) return NotFound();

        if (sourceUser.UserName == username) return BadRequest("You can't like yourself");

        var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId, likedUser.Id);

        if (userLike != null) return BadRequest("You already like this user");

        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            LikedUserId = likedUser.Id
        };

        sourceUser.LikedUsers.Add(userLike);

        // if(await _unitOfWork.Complete()) return Ok();
        if (await _unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to like user");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
        return Ok(users);
    }
}
