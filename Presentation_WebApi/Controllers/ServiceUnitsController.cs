using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/service-units")]
[ApiController]
public class ServiceUnitsController(IServiceUnitService serviceUnitService) : ControllerBase
{
    private readonly IServiceUnitService _serviceUnitService = serviceUnitService;

    // Add service unit
    [HttpPost]
    public async Task<IActionResult> Add(ServiceUnitRegistrationForm form)
    {
        var result = await _serviceUnitService.CreateServiceUnitAsync(form);

        return result.Success
            ? CreatedAtAction(nameof(Add), ((ServiceResult<ServiceUnitModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // Get all service units
    [HttpGet]
    public async Task<IActionResult> GetAllServiceUnits()
    {
        var result = await _serviceUnitService.GetAllServiceUnitsAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<ServiceUnitModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get service unit by id
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetServiceUnitById(int id)
    {
        var result = await _serviceUnitService.GetServiceUnitByIdAsync(id);

        return result.Success
            ? Ok(((ServiceResult<ServiceUnitModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update service unit
    [HttpPut()]
    public async Task<IActionResult> Update(ServiceUnitUpdateForm form)
    {
        var result = await _serviceUnitService.UpdateServiceUnitAsync(form);

        return result.Success
            ? Ok(((ServiceResult<ServiceUnitModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete service unit
    [HttpDelete]
    public async Task<IActionResult> Delete(ServiceUnitModel form)
    {
        var result = await _serviceUnitService.DeleteServiceUnitAsync(form);

        return result.Success
            ? Ok("Service unit deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
