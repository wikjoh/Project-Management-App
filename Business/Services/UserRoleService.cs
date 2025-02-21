using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;

namespace Business.Services;

public class UserRoleService(IUserRoleRepository userRoleRepository) : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository = userRoleRepository;


    // CREATE
    public async Task<IServiceResult> AddRoleAsync(int userId, int roleId)
    {
        bool? userRoleExists = await _userRoleRepository.ExistsAsync(x => x.UserId == userId && x.RoleId == roleId);
        if (userRoleExists == true)
            return ServiceResult.AlreadyExists($"UserId {userId} already has roleId {roleId}.");
        if (userRoleExists == null)
            return ServiceResult.InternalServerError("Failed verifying if user role exists.");

        var userRoleEntity = UserRoleFactory.Create(userId, roleId);

        await _userRoleRepository.CreateAsync(userRoleEntity);
        var result = await _userRoleRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating user role.");

        var userRole = UserRoleFactory.ToModel(userRoleEntity);
        return ServiceResult<UserRoleModel>.Ok(userRole);
    }


    // READ
    public async Task<IServiceResult> GetAllRolesByUserIdAsync(int userId)
    {
        var userRoles = await _userRoleRepository.GetAllWhereAsync(x => x.UserId == userId);
        var userRolesList = userRoles != null ? userRoles.Select(x => UserRoleFactory.ToModel(x)) : [];

        return ServiceResult<IEnumerable<UserRoleModel>>.Ok(userRolesList);
    }

    public async Task<IServiceResult> GetAllUsersByRoleIdAsync(int roleId)
    {
        var userRoles = await _userRoleRepository.GetAllWhereAsync(x => x.RoleId == roleId);
        var userRolesList = userRoles != null ? userRoles.Select(x => UserRoleFactory.ToModel(x)) : [];

        return ServiceResult<IEnumerable<UserRoleModel>>.Ok(userRolesList);
    }


    // DELETE
    public async Task<IServiceResult> DeleteUserRoleAsync(UserRoleModel model)
    {
        if (model == null)
            return ServiceResult.BadRequest("Parameter cannot be empty.");

        var userRoleEntity = await _userRoleRepository.GetOneAsync(x => x.UserId == model.UserId && x.RoleId == model.RoleId);
        if (userRoleEntity == null)
            return ServiceResult.NotFound("User role not found.");

        _userRoleRepository.Delete(userRoleEntity);
        var result = await _userRoleRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting user role failed.");

        return ServiceResult.Ok();
    }
}
