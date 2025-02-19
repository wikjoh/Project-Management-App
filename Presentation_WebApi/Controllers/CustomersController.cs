﻿using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/customers")]
[ApiController]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    // Create Customer with phone number
    [HttpPost]
    public async Task<IActionResult> Create(CustomerRegistrationForm form)
    {
        var result = await _customerService.CreateCustomerAsync(form);

        return result.Success
            ? CreatedAtAction(nameof(this.Create), ((ServiceResult<CustomerModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all customers including phone numbers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerService.GetAllWithPhoneAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<CustomerModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get customer by email including phone number
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _customerService.GetByEmailWithPhoneAsync(email);

        return result.Success
            ? Ok(((ServiceResult<CustomerModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get customer by id including phone number
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdWithPhoneAsync(id);

        return result.Success
            ? Ok(((ServiceResult<CustomerModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update customer
    [HttpPut()]
    public async Task<IActionResult> Update(CustomerUpdateForm form)
    {
        var result = await _customerService.UpdateCustomerAsync(form);

        return result.Success
            ? Ok(((ServiceResult<CustomerModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete customer by email
    [HttpDelete("email/{email}")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        var result = await _customerService.DeleteCustomerByEmailAsync(email);

        return result.Success
            ? Ok("Customer deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete customer by id
    [HttpDelete("id/{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await _customerService.DeleteCustomerByIdAsync(id);

        return result.Success
            ? Ok("Customer deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
