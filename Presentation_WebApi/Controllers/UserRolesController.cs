using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/user-roles")]
[ApiController]
public class UserRolesController(IUserRoleService userRoleService) : ControllerBase
{
    private readonly IUserRoleService _userRoleService = userRoleService;


    // Add user role
    [HttpPost]
    public async Task<IActionResult> Add(UserRoleModel model)
    {
        var result = await _userRoleService.AddRoleAsync(model.UserId, model.RoleId);

        return result.Success
            ? CreatedAtAction(nameof(Add), ((ServiceResult<UserRoleModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // Get all roles by userid
    [HttpGet("user-id/{userid}")]
    public async Task<IActionResult> GetAllRolesByUserId(int userid)
    {
        var result = await _userRoleService.GetAllRolesByUserIdAsync(userid);

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<UserRoleModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all users by roleid
    [HttpGet("role-id/{roleid}")]
    public async Task<IActionResult> GetAllUsersByRoleId(int roleid)
    {
        var result = await _userRoleService.GetAllUsersByRoleIdAsync(roleid);

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<UserRoleModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // Delete role
    [HttpDelete]
    public async Task<IActionResult> DeleteUserRole(UserRoleModel model)
    {
        var result = await _userRoleService.DeleteUserRoleAsync(model);

        return result.Success
            ? Ok("User role deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
