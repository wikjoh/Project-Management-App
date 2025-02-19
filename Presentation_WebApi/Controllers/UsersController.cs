﻿using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/users")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    // Create user with role
    [HttpPost]
    public async Task<IActionResult> Create(UserRegistrationForm form)
    {
        var result = await _userService.CreateUser(form);

        return result.Success
            ? CreatedAtAction(nameof(Create), ((ServiceResult<UserModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all users including roles
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllWithRolesAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<UserModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get user by id including roles
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetByIdWithRolesAsync(id);

        return result.Success
            ? Ok(((ServiceResult<UserModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get user by email including roles
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetById(string email)
    {
        var result = await _userService.GetByEmailWithRolesAsync(email);

        return result.Success
            ? Ok(((ServiceResult<UserModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // Update user
    [HttpPut]
    public async Task<IActionResult> Update(UserUpdateForm form)
    {
        var result = await _userService.UpdateUserAsync(form);

        return result.Success
            ? Ok(((ServiceResult<UserModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete user by user model
    [HttpDelete]
    public async Task<IActionResult> Delete(UserModel model)
    {
        var result = await _userService.DeleteUserAsync(model);

        return result.Success
            ? Ok("User deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
