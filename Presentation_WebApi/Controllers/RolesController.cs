using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/roles")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;


    // Add role
    [HttpPost]
    public async Task<IActionResult> Add(RoleRegistrationForm form)
    {
        var result = await _roleService.CreateRoleAsync(form);

        return result.Success
            ? CreatedAtAction(nameof(Add), ((ServiceResult<RoleModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);

  
    }


    // Get all roles
    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRolesAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<RoleModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get role by id
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetRoleByIdAsync(int id)
    {
        var result = await _roleService.GetRoleByIdAsync(id);

        return result.Success
            ? Ok(((ServiceResult<RoleModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update role
    [HttpPut]
    public async Task<IActionResult> UpdateRoleAsync(RoleUpdateForm form)
    {
        var result = await _roleService.UpdateRoleAsync(form);

        return result.Success
            ? Ok(((ServiceResult<RoleModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete role
    [HttpDelete]
    public async Task<IActionResult> DeleteRole(RoleModel form)
    {
        var result = await _roleService.DeleteRoleAsync(form);

        return result.Success
            ? Ok("Role deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
