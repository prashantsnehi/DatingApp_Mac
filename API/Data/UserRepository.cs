using AutoMapper.QueryableExtensions;
namespace API.Data;
public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<AppUser> GetUserByIdAsync(int id) => await _context.Users.FindAsync(id);

    public async Task<AppUser> GetUserByUsernameAsync(string username) =>
                        await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    // public async Task<bool> SaveAllAsync() => await _context.SaveChangesAsync() > 0;

    public async Task<IEnumerable<AppUser>> GetUsersAsync() => await _context.Users.Include(p => p.Photos).ToListAsync();

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = _context.Users.AsQueryable();

        var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
        var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

        query = query.Where(u => u.UserName != userParams.CurrentUserName);
        query = query.Where(u => u.Gender.ToLower() == userParams.Gender);
        query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(u => u.Created),
            _ => query.OrderByDescending(u => u.LastActive)
        };
        
        return await PagedList<MemberDto>
                    .CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), 
                                 userParams.PageNumber, userParams.PageSize);
    }

    public async Task<MemberDto> GetMemberAsync(string username, bool? isCurrentUser) 
    {
        var query =  _context.Users
                        .Where(x => x.UserName == username)
                        .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                        .AsQueryable();
        if (isCurrentUser.Value) query = query.IgnoreQueryFilters();

        return await query.FirstOrDefaultAsync();
    }

    public async Task<string> GetUserGender(string username) => 
           await _context.Users.Where(x => x.UserName == username).Select(x => x.Gender).FirstOrDefaultAsync();

    public async Task<AppUser> GetUserByPhotoId(int photoId) => 
            await _context.Users
                .Include(p => p.Photos)
                .IgnoreQueryFilters()
                .Where(p => p.Photos.Any(p => p.Id == photoId))
                .FirstOrDefaultAsync();
}