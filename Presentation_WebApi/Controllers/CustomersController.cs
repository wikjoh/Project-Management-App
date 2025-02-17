using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/customers")]
[ApiController]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    // Create Customer with phone number
    [HttpPost]
    public async Task<IServiceResult> Create(CustomerRegistrationForm form)
    {
        var result = await _customerService.CreateCustomerAsync(form);
        return result;
    }

    // Get all customers including phone numbers
    [HttpGet]
    public async Task<IServiceResult> GetAll()
    {
        var result = await _customerService.GetAllWithPhoneAsync();
        return result;
    }

    // Get customer by email including phone number
    [HttpGet("email/{email}")]
    public async Task<IServiceResult> GetByEmail(string email)
    {
        var result = await _customerService.GetByEmailWithPhoneAsync(email);
        return result;
    }

    // Get customer by id including phone number
    [HttpGet("id/{id}")]
    public async Task<IServiceResult> GetById(int id)
    {
        var result = await _customerService.GetByIdWithPhoneAsync(id);
        return result;
    }

    // Update customer
    [HttpPut()]
    public async Task<IServiceResult> Update(CustomerUpdateForm form)
    {
        var result = await _customerService.UpdateCustomerAsync(form);
        return result;
    }

    // Delete customer by email
    [HttpDelete("email/{email}")]
    public async Task<IServiceResult> DeleteByEmail(string email)
    {
        var result = await _customerService.DeleteCustomerByEmailAsync(email);
        return result;
    }

    // Delete customer by id
    [HttpDelete("id/{id}")]
    public async Task<IServiceResult> DeleteById(int id)
    {
        var result = await _customerService.DeleteCustomerByIdAsync(id);
        return result;
    }
}
