using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/services")]
[ApiController]
public class ServicesController(IServiceService serviceService) : ControllerBase
{
    private readonly IServiceService _serviceService = serviceService;

    // Add service
    [HttpPost]
    public async Task<IActionResult> Add(ServiceRegistrationForm form)
    {
        var result = await _serviceService.CreateServiceAsync(form);

        return result.Success
            ? CreatedAtAction(nameof(Add), ((ServiceResult<ServiceModelDetailed>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // Get all services
    [HttpGet]
    public async Task<IActionResult> GetAllServices()
    {
        var result = await _serviceService.GetAllServicesAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<ServiceModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all services detailed
    [HttpGet("detailed")]
    public async Task<IActionResult> GetAllServicesDetailed()
    {
        var result = await _serviceService.GetAllServicesDetailedAsync();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<ServiceModelDetailed>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get service by id
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetServiceById(int id)
    {
        var result = await _serviceService.GetServiceByIdDetailedAsync(id);

        return result.Success
            ? Ok(((ServiceResult<ServiceModelDetailed>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update service
    [HttpPut()]
    public async Task<IActionResult> Update(ServiceUpdateForm form)
    {
        var result = await _serviceService.UpdateServiceAsync(form);

        return result.Success
            ? Ok(((ServiceResult<ServiceModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete service
    [HttpDelete]
    public async Task<IActionResult> Delete(ServiceModel form)
    {
        var result = await _serviceService.DeleteServiceAsync(form);

        return result.Success
            ? Ok("Service  deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
