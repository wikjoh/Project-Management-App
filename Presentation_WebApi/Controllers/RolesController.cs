using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/roles")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;


    // Add role
    [HttpPost]
    public async Task<IServiceResult> Add(RoleRegistrationForm form)
    {
        var result = await _roleService.CreateRoleAsync(form);
        return result;
    }


    // Get all roles
    [HttpGet]
    public async Task<IServiceResult> GetAllRoles()
    {
        var result = await _roleService.GetAllRolesAsync();
        return result;
    }


    // Delete role
    [HttpDelete]
    public async Task<IServiceResult> DeleteRole(RoleModel form)
    {
        var result = await _roleService.DeleteRoleAsync(form);
        return result;
    }
}
