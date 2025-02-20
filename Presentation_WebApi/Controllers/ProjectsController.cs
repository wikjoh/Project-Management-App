using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_WebApi.Controllers;
[Route("api/projects")]
[ApiController]
public class ProjectsController(IProjectService projectService) : ControllerBase
{
    private readonly IProjectService _projectService = projectService;

    // Add Project
    [HttpPost]
    public async Task<IActionResult> Add(ProjectRegistrationForm form)
    {
        var result = await _projectService.CreateProject(form);

        return result.Success
            ? CreatedAtAction(nameof(Add), ((ServiceResult<ProjectModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get all projects
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var result = await _projectService.GetAllProjects();

        return result.Success
            ? Ok(((ServiceResult<IEnumerable<ProjectModel>>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Get project by id
    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var result = await _projectService.GetProjectById(id);

        return result.Success
            ? Ok(((ServiceResult<ProjectModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Update project
    [HttpPut]
    public async Task<IActionResult> Update(ProjectUpdateForm form)
    {
        var result = await _projectService.UpdateProject(form);

        return result.Success
            ? Ok(((ServiceResult<ProjectModel>)result).Data)
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    // Delete project
    [HttpDelete]
    public async Task<IActionResult> Delete(ProjectModel form)
    {
        var result = await _projectService.DeleteProject(form);

        return result.Success
            ? Ok("Project deleted successfully.")
            : StatusCode(result.StatusCode, result.ErrorMessage);
    }
}
