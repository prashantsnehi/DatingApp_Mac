namespace API.Data;

public class PhotoRepository : IPhotoRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public PhotoRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Photo> GetPhotoById(int id) =>
        await _context.Photos.IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos() =>
        await _context.Photos
            .IgnoreQueryFilters()
            .Where(p => p.IsApproved == false)
            .Select(u => new PhotoForApprovalDto
            {
                Id = u.Id,
                Username = u.AppUser.UserName,
                Url = u.Url,
                IsApproved = u.IsApproved
            })
            .ToListAsync();

    public void RemovePhoto(Photo photo)
    {
        _context.Photos.Remove(photo);
    }
}
