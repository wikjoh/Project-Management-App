using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Business.Services;

public class UserService(IUserRepository userRepository, IUserRoleService userRoleService) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserRoleService _userRoleService = userRoleService;


    // CREATE
    public async Task<IServiceResult> CreateUser(UserRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Invalid registration form.");

        bool? customerExistss = await _userRepository.ExistsAsync(x => x.EmailAddress == form.EmailAddress);
        if (customerExistss == true)
            return ServiceResult.AlreadyExists("User with given email address already exists.");

        var userEntity = UserFactory.Create(form);

        await _userRepository.BeginTransactionAsync();

        try
        {
            await _userRepository.CreateAsync(userEntity);
            var userResult = await _userRepository.SaveAsync() > 0;
            if (!userResult)
                throw new Exception("Failed to create user entity.");

            foreach (var roleId in form.RoleIds)
            {
                var roleResult = await _userRoleService.AddRoleAsync(userEntity.Id, roleId);
                if (!roleResult.Success)
                    throw new Exception("Failed to create user role entity.");
            }

            await _userRepository.CommitTransactionAsync();
            var createdUserWithUserRoles = UserFactory.Create((await _userRepository.GetOneAsync(x => x.Id == userEntity.Id, q => q.Include(u => u.UserRoles)))!);
            return ServiceResult<UserModel>.Created(createdUserWithUserRoles);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed creating user. Rolling back. {ex.Message}";
            Debug.WriteLine(errorMessage);
            await _userRepository.RollbackTransactionAsync();
            return ServiceResult.InternalServerError(errorMessage);
        }
    }


    // READ
    public async Task<IServiceResult> GetAllWithRolesAsync()
    {
        var users = await _userRepository.GetAllAsync(q => q.Include(u => u.UserRoles));
        users ??= [];

        var usersList = users.Select(x => UserFactory.Create(x));
        return ServiceResult<IEnumerable<UserModel>>.Ok(usersList);
    }

    public async Task<IServiceResult> GetByIdWithRolesAsync(int id)
    {
        var userEntity = await _userRepository.GetOneAsync(x => x.Id == id, q => q.Include(u => u.UserRoles));

        if (userEntity == null)
            return ServiceResult.NotFound($"User with id {id} not found.");

        var user = UserFactory.Create(userEntity);
        return ServiceResult<UserModel>.Ok(user);
    }

    public async Task<IServiceResult> GetByEmailWithRolesAsync(string email)
    {
        var userEntity = await _userRepository.GetOneAsync(x => x.EmailAddress == email, q => q.Include(u => u.UserRoles));

        if (userEntity == null)
            return ServiceResult.NotFound($"User with email {email} not found.");

        var user = UserFactory.Create(userEntity);
        return ServiceResult<UserModel>.Ok(user);
    }


    // UPDATE
    public async Task<IServiceResult> UpdateUserAsync(UserUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var user = await _userRepository.GetOneAsync(x => x.Id == form.Id);

        if (user == null)
            return ServiceResult.NotFound($"User not found.");

        user.FirstName = form.FirstName;
        user.LastName = form.LastName;
        user.EmailAddress = form.EmailAddress;

        _userRepository.Update(user);
        var result = await _userRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Updating user failed.");

        var userModel = UserFactory.Create((await _userRepository.GetOneAsync(x => x.Id == form.Id))!);
        return userModel != null
            ? ServiceResult<UserModel>.Ok(userModel)
            : ServiceResult.InternalServerError("Retrieved null user after update.");
    }


    // DELETE
    public async Task<IServiceResult> DeleteUserAsync(UserModel model)
    {
        var user = await _userRepository.GetOneAsync(x => x.Id == model.Id);
        if (user == null)
            return ServiceResult.NotFound("User not found.");

        _userRepository.Delete(user);
        var result = await _userRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting user failed.");

        return ServiceResult.Ok();
    }
}
