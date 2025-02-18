using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/customer-phone-numbers")]
[ApiController]
public class CustomerPhoneNumbersController(ICustomerPhoneNumberService customerPhoneNumberService) : ControllerBase
{
    private readonly ICustomerPhoneNumberService _customerPhoneNumberService = customerPhoneNumberService;

    // Add phone number
    [HttpPost]
    public async Task<IServiceResult> Add(CustomerPhoneNumberForm form)
    {
        var result = await _customerPhoneNumberService.AddPhoneNumberAsync(form);
        return result;
    }

    // Get all phone numbers for specified customer id
    [HttpGet("customer-id/{id}")]
    public async Task<IServiceResult> GetCustomerPhoneNumbers(int id)
    {
        var result = await _customerPhoneNumberService.GetAllPhoneNumbersByCustomerIdAsync(id);
        return result;
    }

    // Update phone number
    [HttpPut()]
    public async Task<IServiceResult> Update(CustomerPhoneNumberForm form)
    {
        var result = await _customerPhoneNumberService.UpdatePhoneNumberAsync(form);
        return result;
    }

    // Delete phone number
    [HttpDelete]
    public async Task<IServiceResult> Delete(CustomerPhoneNumberModel form)
    {
        var result = await _customerPhoneNumberService.DeletePhoneNumberAsync(form);
        return result;
    }
}
