using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;


    // CREATE
    public async Task<IServiceResult> CreateProject(ProjectRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var projectEntity = ProjectFactory.ToEntity(form);

        await _projectRepository.CreateAsync(projectEntity);
        var result = await _projectRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating project.");

        var project = ProjectFactory.ToModel(projectEntity);
        return ServiceResult<ProjectModel>.Ok(project);
    }


    // READ
    public async Task<IServiceResult> GetAllProjects()
    {
        var projectEntities = await _projectRepository.GetAllAsync();
        var projectList = projectEntities != null ? projectEntities.Select(x => ProjectFactory.ToModel(x)) : [];

        return ServiceResult<IEnumerable<ProjectModel>>.Ok(projectList);
    }

    public async Task<IServiceResult> GetAllProjectsDetailed()
    {
        var projectEntities = await _projectRepository.GetAllAsync(q => q
                                                            .Include(p => p.Customer)
                                                            .Include(p => p.ProjectManager)
                                                            .Include(p => p.Service)
                                                            .Include(p => p.Status));

        var projectList = projectEntities != null ? projectEntities.Select(x => ProjectFactory.ToModelDetailed(x)) : [];

        return ServiceResult<IEnumerable<ProjectModelDetailed>>.Ok(projectList);
    }

    public async Task<IServiceResult> GetProjectById(int id)
    {
        var projectEntity = await _projectRepository.GetOneAsync(x => x.Id == id);
        if (projectEntity == null)
            return ServiceResult.NotFound($"Project with id {id} not found.");

        var project = ProjectFactory.ToModel(projectEntity);
        return ServiceResult<ProjectModel>.Ok(project);
    }

    public async Task<IServiceResult> GetProjectByIdDetailed(int id)
    {
        var projectEntity = await _projectRepository.GetOneAsync(x => x.Id == id, q => q
                                                            .Include(p => p.Customer)
                                                            .Include(p => p.ProjectManager)
                                                            .Include(p => p.Service)
                                                            .Include(p => p.Status));

        if (projectEntity == null)
            return ServiceResult.NotFound($"Project with id {id} not found.");

        var project = ProjectFactory.ToModelDetailed(projectEntity);
        return ServiceResult<ProjectModelDetailed>.Ok(project);
    }


    // UPDATE
    public async Task<IServiceResult> UpdateProject(ProjectUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var existingEntity = await _projectRepository.GetOneAsync(x => x.Id == form.Id);
        if (existingEntity == null)
            return ServiceResult.NotFound("Project not found.");

        existingEntity.Name = form.Name;
        existingEntity.StartDate = form.StartDate;
        existingEntity.EndDate = form.EndDate;
        existingEntity.ProjectManagerId = form.ProjectManagerId;
        existingEntity.CustomerName = form.CustomerName;
        existingEntity.CustomerId = form.CustomerId;
        existingEntity.ServiceId = form.ServiceId;
        existingEntity.ServiceQuantity = form.ServiceQuantity;
        existingEntity.TotalPrice = form.TotalPrice;
        existingEntity.StatusId = form.StatusId;

        _projectRepository.Update(existingEntity);
        var result = await _projectRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed updating project.");

        var updatedEntity = await _projectRepository.GetOneAsync(x => x.Id == form.Id);
        if (updatedEntity == null)
            return ServiceResult.InternalServerError("Retrieved null entity after update.");

        var updatedModel = ProjectFactory.ToModel(updatedEntity);
        return ServiceResult<ProjectModel>.Ok(updatedModel);
    }


    // DELETE
    public async Task<IServiceResult> DeleteProject(ProjectModel model)
    {
        if (model == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var projectEntity = await _projectRepository.GetOneAsync(x => x.Id == model.Id);
        if (projectEntity == null)
            return ServiceResult.NotFound("Project not found.");

        _projectRepository.Delete(projectEntity);
        var result = await _projectRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed deleting project.");

        return ServiceResult.Ok();
    }
}
