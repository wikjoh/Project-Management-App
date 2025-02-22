using Business.Dtos;
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
            ? CreatedAtAction(nameof(Create), ((ServiceResult<CustomerModelDetailed>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all customers 
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerService.GetAllAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<CustomerModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all customers detailed
    [HttpGet("detailed")]
    public async Task<IActionResult> GetAllDetailed()
    {
        var result = await _customerService.GetAllDetailedAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<CustomerModelDetailed>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get customer by email
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _customerService.GetByEmailAsync(email);

        return result.Success
            ? Ok(((ServiceResult<CustomerModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get customer detailed by email
    [HttpGet("detailed/email/{email}")]
    public async Task<IActionResult> GetByEmailDetailed(string email)
    {
        var result = await _customerService.GetByEmailDetailedAsync(email);

        return result.Success
            ? Ok(((ServiceResult<CustomerModelDetailed>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get customer by id
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdAsync(id);

        return result.Success
            ? Ok(((ServiceResult<CustomerModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get customer detailed by id
    [HttpGet("detailed/id/{id}")]
    public async Task<IActionResult> GetByIdDetailed(int id)
    {
        var result = await _customerService.GetByIdDetailedAsync(id);

        return result.Success
            ? Ok(((ServiceResult<CustomerModelDetailed>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update customer
    [HttpPut()]
    public async Task<IActionResult> Update(CustomerUpdateForm form)
    {
        var result = await _customerService.UpdateCustomerAsync(form);

        return result.Success
            ? Ok(((ServiceResult<CustomerModelDetailed>)result).Data)
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
