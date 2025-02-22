using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/customer-phone-numbers")]
[ApiController]
public class CustomerPhoneNumbersController(ICustomerPhoneNumberService customerPhoneNumberService) : ControllerBase
{
    private readonly ICustomerPhoneNumberService _customerPhoneNumberService = customerPhoneNumberService;

    // Add phone number
    [HttpPost]
    public async Task<IActionResult> Add(CustomerPhoneNumberRegistrationForm form)
    {
        var result = await _customerPhoneNumberService.AddPhoneNumberAsync(form);
        
        return result.Success
            ? CreatedAtAction(nameof(Add), ((ServiceResult<CustomerPhoneNumberModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all phone numbers for specified customer id
    [HttpGet("customer-id/{id}")]
    public async Task<IActionResult> GetCustomerPhoneNumbers(int id)
    {
        var result = await _customerPhoneNumberService.GetAllPhoneNumbersByCustomerIdAsync(id);

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<CustomerPhoneNumberModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update phone number
    [HttpPut()]
    public async Task<IActionResult> Update(CustomerPhoneNumberUpdateForm form)
    {
        var result = await _customerPhoneNumberService.UpdatePhoneNumberAsync(form);

        return result.Success
            ? Ok(((ServiceResult<CustomerPhoneNumberModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete phone number
    [HttpDelete]
    public async Task<IActionResult> Delete(CustomerPhoneNumberModel form)
    {
        var result = await _customerPhoneNumberService.DeletePhoneNumberAsync(form);

        return result.Success
            ? Ok("Phone number deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
