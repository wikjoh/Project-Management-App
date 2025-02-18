﻿using Business.Dtos;
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

        var roleEntity = RoleFactory.Create(form);

        await _roleRepository.CreateAsync(roleEntity);
        var result = await _roleRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating role.");

        var role = RoleFactory.Create(roleEntity);
        return ServiceResult<RoleModel>.Ok(role);
    }


    // READ
    public async Task<IServiceResult> GetAllRoles()
    {
        var roles = await _roleRepository.GetAllAsync();

        var roleList = roles.Select(x => RoleFactory.Create(x));
        return ServiceResult<IEnumerable<RoleModel>>.Ok(roleList);
    }



    // DELETE
    public async Task<IServiceResult> DeleteById(int id)
    {
        var role = await _roleRepository.GetOneAsync(x => x.Id == id);
        if (role == null)
            return ServiceResult.NotFound($"Role with id {id} does not exist.");

        _roleRepository.Delete(role);
        var result = await _roleRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting role failed.");

        return ServiceResult.Ok();
    }
}
