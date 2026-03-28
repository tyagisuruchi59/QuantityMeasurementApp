using QuantityMeasurementAppModel.Entities;

namespace QuantityMeasurementAppRepositoryLayer.Interface
{
    /// <summary>
    /// Repository interface for UserEntity persistence.
    /// Handles all user-related database operations.
    /// </summary>
    public interface IUserRepository
    {
        Task<UserEntity> SaveAsync(UserEntity user);
        Task<UserEntity?> FindByEmailAsync(string email);
        Task<UserEntity?> FindByUsernameAsync(string username);
        Task<UserEntity?> FindByIdAsync(long id);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<UserEntity> UpdateAsync(UserEntity user);
        Task<List<UserEntity>> FindAllAsync();
    }
}