using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
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
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _customerService.CreateCustomerAsync(form);
        return result ? Ok() : Problem();
    }

    // Get all customers including phone numbers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _customerService.GetAllWithPhoneAsync();
        return Ok(result);
    }

    // Get customer by email including phone number
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var result = await _customerService.GetByEmailWithPhoneAsync(email);
        return result != null ? Ok(result) : NotFound();
    }

    // Get customer by id including phone number
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _customerService.GetByIdWithPhoneAsync(id);
        return result != null ? Ok(result) : NotFound();
    }

    // Update customer
    [HttpPut()]
    public async Task<IActionResult> Update(CustomerUpdateForm form)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _customerService.UpdateCustomerAsync(form);
        return result != null ? Ok(result) : NotFound();
    }

    // Delete customer by email
    [HttpDelete("email/{email}")]
    public async Task<IActionResult> DeleteByEmail(string email)
    {
        var result = await _customerService.DeleteCustomerByEmailAsync(email);
        return result ? Ok("Customer deleted successfully.") : NotFound();
    }

    // Delete customer by id
    [HttpDelete("id/{id}")]
    public async Task<IActionResult> DeleteById(int id)
    {
        var result = await _customerService.DeleteCustomerByIdAsync(id);
        return result ? Ok("Customer deleted successfully.") : NotFound();
    }
}
