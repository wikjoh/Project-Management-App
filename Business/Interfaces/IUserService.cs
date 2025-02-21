using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IUserService
{
    Task<IServiceResult> CreateUser(UserRegistrationForm form);
    Task<IServiceResult> DeleteUserAsync(UserModel model);
    Task<IServiceResult> GetAll();
    Task<IServiceResult> GetAllDetailedAsync();
    Task<IServiceResult> GetByEmailDetailedAsync(string email);
    Task<IServiceResult> GetByIdDetailedAsync(int id);
    Task<IServiceResult> UpdateUserAsync(UserUpdateForm form);
}