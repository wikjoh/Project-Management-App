using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/user-roles")]
[ApiController]
public class UserRolesController(IUserRoleService userRoleService) : ControllerBase
{
    private readonly IUserRoleService _userRoleService = userRoleService;


    // Add user role
    [HttpPost]
    public async Task<IServiceResult> Add(UserRoleModel model)
    {
        var result = await _userRoleService.AddRoleAsync(model.UserId, model.RoleId);
        return result;
    }


    // Get all roles by userid
    [HttpGet("user-id/{userid}")]
    public async Task<IServiceResult> GetAllRolesByUserId(int userid)
    {
        var result = await _userRoleService.GetAllRolesByUserIdAsync(userid);
        return result;
    }

    // Get all users by roleid
    [HttpGet("role-id/{roleid}")]
    public async Task<IServiceResult> GetAllUsersByRoleId(int roleid)
    {
        var result = await _userRoleService.GetAllUsersByRoleIdAsync(roleid);
        return result;
    }


    // Delete role
    [HttpDelete]
    public async Task<IServiceResult> DeleteUserRole(UserRoleModel model)
    {
        var result = await _userRoleService.DeleteUserRoleAsync(model);
        return result;
    }
}
