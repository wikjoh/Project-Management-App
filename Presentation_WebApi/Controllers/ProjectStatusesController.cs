using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/project-statuses")]
[ApiController]
public class ProjectStatusesController(IProjectStatusService projectStatusService) : ControllerBase
{
    private readonly IProjectStatusService _projectStatusService = projectStatusService;

    // Add project status
    [HttpPost]
    public async Task<IActionResult> Add(ProjectStatusRegistrationForm form)
    {
        var result = await _projectStatusService.CreateProjectStatusAsync(form);

        return result.Success
            ? CreatedAtAction(nameof(Add), ((ServiceResult<ProjectStatusModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    // Get all project statuses
    [HttpGet]
    public async Task<IActionResult> GetAllProjectStatuses()
    {
        var result = await _projectStatusService.GetAllProjectStatuses();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<ProjectStatusModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get project status by id
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetProjectStatusById(int id)
    {
        var result = await _projectStatusService.GetProjectStatusByIdAsync(id);

        return result.Success
            ? Ok(((ServiceResult<ProjectStatusModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update service unit
    [HttpPut()]
    public async Task<IActionResult> Update(ProjectStatusUpdateForm form)
    {
        var result = await _projectStatusService.UpdateProjectStatusAsync(form);

        return result.Success
            ? Ok(((ServiceResult<ProjectStatusModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete service unit
    [HttpDelete]
    public async Task<IActionResult> Delete(ProjectStatusModel form)
    {
        var result = await _projectStatusService.DeleteProjectStatusAsync(form);

        return result.Success
            ? Ok("Service unit deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
