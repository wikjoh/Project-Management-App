using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;

namespace Business.Services;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    private readonly IRoleRepository _roleRepository = roleRepository;


    // CREATE
    public async Task<IServiceResult> CreateRoleAsync(RoleRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Invalid registration form.");

        bool? roleExists = await _roleRepository.ExistsAsync(x => x.Role == form.Role);
        if (roleExists == true)
            return ServiceResult.AlreadyExists($"Role with name {form.Role} already exists.");
        if (roleExists == null)
            return ServiceResult.InternalServerError("Failed verifying if role already exists");

        var roleEntity = RoleFactory.ToEntity(form);

        await _roleRepository.CreateAsync(roleEntity);
        var result = await _roleRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating role.");

        var role = RoleFactory.ToModel(roleEntity);
        return ServiceResult<RoleModel>.Ok(role);
    }


    // READ
    public async Task<IServiceResult> GetAllRolesAsync()
    {
        var roles = await _roleRepository.GetAllAsync();

        var roleList = roles.Select(x => RoleFactory.ToModel(x));
        return ServiceResult<IEnumerable<RoleModel>>.Ok(roleList);
    }

    public async Task<IServiceResult> GetRoleByIdAsync(int id)
    {
        var roleEntity = await _roleRepository.GetOneAsync(x => x.Id == id);
        if (roleEntity == null)
            return ServiceResult.NotFound($"Role with id {id} not found.");

        var role = RoleFactory.ToModel(roleEntity);
        return ServiceResult<RoleModel>.Ok(role);
    }


    // UPDATE
    public async Task<IServiceResult> UpdateRoleAsync(RoleUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var existingEntity = await _roleRepository.GetOneAsync(x => x.Id == form.Id);
        if (existingEntity == null)
            return ServiceResult.NotFound("Role not found.");

        existingEntity.Role = form.Role;

        _roleRepository.Update(existingEntity);
        var result = await _roleRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed to update role.");

        var updatedEntity = await _roleRepository.GetOneAsync(x => x.Id == form.Id);
        if (updatedEntity == null)
            return ServiceResult.InternalServerError("Returned null entity after update.");

        var updated = RoleFactory.ToModel(updatedEntity);
        return ServiceResult<RoleModel>.Ok(updated);
    }


    // DELETE
    public async Task<IServiceResult> DeleteRoleAsync(RoleModel model)
    {
        if (model == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var role = await _roleRepository.GetOneAsync(x => x.Id == model.Id);
        if (role == null)
            return ServiceResult.NotFound($"Role with id {model.Id} does not exist.");

        _roleRepository.Delete(role);
        var result = await _roleRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting role failed.");

        return ServiceResult.Ok();
    }
}
