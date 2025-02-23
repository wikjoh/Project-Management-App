using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IRoleService
{
    Task<IServiceResult> CreateRoleAsync(RoleRegistrationForm form);
    Task<IServiceResult> DeleteRoleAsync(RoleModel model);
    Task<IServiceResult> GetAllRolesAsync();
    Task<IServiceResult> GetRoleByIdAsync(int id);
    Task<IServiceResult> UpdateRoleAsync(RoleUpdateForm form);
}