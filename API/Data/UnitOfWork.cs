using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public UnitOfWork(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

        public ILikesRepository LikesRepository => new LikesRepository(_userManager, _context);

        public async Task<bool> Complete() => await _context.SaveChangesAsync() > 0;

        public bool HasChanges() => _context.ChangeTracker.HasChanges();
    }
}