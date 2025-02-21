using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models.ServiceResult;
using Business.Models;
using Data.Interfaces;
using Data.Repositories;

namespace Business.Services;

public class ProjectStatusService(IProjectStatusRepository projectStatusRepository) : IProjectStatusService
{
    private readonly IProjectStatusRepository _projectStatusRepository = projectStatusRepository;


    // CREATE
    public async Task<IServiceResult> CreateProjectStatusAsync(ProjectStatusRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        bool? projectStatusExists = await _projectStatusRepository.ExistsAsync(x => x.Name == form.Name);
        if (projectStatusExists == true)
            return ServiceResult.AlreadyExists($"Project status {form.Name} already exists.");
        if (projectStatusExists == null)
            return ServiceResult.InternalServerError("Failed verifying if project status exists.");

        var entity = ProjectStatusFactory.ToEntity(form);

        await _projectStatusRepository.CreateAsync(entity);
        var result = await _projectStatusRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating service status.");

        var projectStatus = ProjectStatusFactory.ToModel(entity);
        return ServiceResult<ProjectStatusModel>.Ok(projectStatus);
    }


    // READ
    public async Task<IServiceResult> GetAllProjectStatuses()
    {
        var projectStatusEntities = await _projectStatusRepository.GetAllAsync();
        var projectStatusList = projectStatusEntities != null ? projectStatusEntities.Select(x => ProjectStatusFactory.ToModel(x)) : [];

        return ServiceResult<IEnumerable<ProjectStatusModel>>.Ok(projectStatusList);
    }

    public async Task<IServiceResult> GetProjectStatusByIdAsync(int id)
    {
        var projectStatusEntity = await _projectStatusRepository.GetOneAsync(x => x.Id == id);

        if (projectStatusEntity == null)
            return ServiceResult.NotFound($"Service status with id {id} not found");

        var projectStatus = ProjectStatusFactory.ToModel(projectStatusEntity);
        return ServiceResult<ProjectStatusModel>.Ok(projectStatus);
    }


    // UPDATE
    public async Task<IServiceResult> UpdateProjectStatusAsync(ProjectStatusUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var existingEntity = await _projectStatusRepository.GetOneAsync(x => x.Id == form.Id);

        if (existingEntity == null)
            return ServiceResult.NotFound("Project status not found.");

        existingEntity.Name = form.Name;

        _projectStatusRepository.Update(existingEntity);
        var result = await _projectStatusRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed updating project status.");

        var updatedEntity = await _projectStatusRepository.GetOneAsync(x => x.Id == form.Id);
        if (updatedEntity == null)
            return ServiceResult.InternalServerError("Retrieved null entity after update.");

        var updatedModel = ProjectStatusFactory.ToModel(updatedEntity);
        return ServiceResult<ProjectStatusModel>.Ok(updatedModel);
    }


    // DELETE
    public async Task<IServiceResult> DeleteProjectStatusAsync(ProjectStatusModel model)
    {
        var projectStatusEntity = await _projectStatusRepository.GetOneAsync(x => x.Id == model.Id);
        if (projectStatusEntity == null)
            return ServiceResult.NotFound("Project status not found");

        _projectStatusRepository.Delete(projectStatusEntity);
        var result = await _projectStatusRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed deleting project status.");

        return ServiceResult.Ok();
    }
}
