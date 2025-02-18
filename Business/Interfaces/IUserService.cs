using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IUserService
{
    Task<IServiceResult> CreateUser(UserRegistrationForm form);
    Task<IServiceResult> DeleteUserAsync(UserModel model);
    Task<IServiceResult> GetAllWithRolesAsync();
    Task<IServiceResult> GetByEmailWithRolesAsync(string email);
    Task<IServiceResult> GetByIdWithRolesAsync(int id);
    Task<IServiceResult> UpdateUserAsync(UserUpdateForm form);
}