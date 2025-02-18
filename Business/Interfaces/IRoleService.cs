using Business.Dtos;

namespace Business.Interfaces;
public interface IRoleService
{
    Task<IServiceResult> CreateRoleAsync(RoleRegistrationForm form);
    Task<IServiceResult> DeleteById(int id);
    Task<IServiceResult> GetAllRoles();
}