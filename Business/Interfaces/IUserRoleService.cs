using Business.Models;

namespace Business.Interfaces;
public interface IUserRoleService
{
    Task<IServiceResult> AddRoleAsync(int userId, int roleId);
    Task<IServiceResult> DeleteUserRoleAsync(UserRoleModel model);
    Task<IServiceResult> GetAllRolesByUserIdAsync(int userId);
    Task<IServiceResult> GetAllUsersByRoleIdAsync(int roleId);
}