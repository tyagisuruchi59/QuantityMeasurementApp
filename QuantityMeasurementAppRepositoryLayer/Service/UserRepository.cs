using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuantityMeasurementAppModel.Entities;
using QuantityMeasurementAppRepositoryLayer.Context;
using QuantityMeasurementAppRepositoryLayer.Interface;

namespace QuantityMeasurementAppRepositoryLayer.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly QuantityDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(QuantityDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserEntity> SaveAsync(UserEntity user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User created: {Email}", user.Email);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving user: {Email}", user.Email);
                throw;
            }
        }

        public async Task<UserEntity?> FindByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<UserEntity?> FindByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<UserEntity?> FindByIdAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<UserEntity> UpdateAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User updated: {Email}", user.Email);
            return user;
        }

        public async Task<List<UserEntity>> FindAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.Username)
                .ToListAsync();
        }
    }
}