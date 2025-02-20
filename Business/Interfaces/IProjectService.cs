using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IProjectService
{
    Task<IServiceResult> CreateProject(ProjectRegistrationForm form);
    Task<IServiceResult> DeleteProject(ProjectModel model);
    Task<IServiceResult> GetAllProjects();
    Task<IServiceResult> GetProjectById(int id);
    Task<IServiceResult> UpdateProject(ProjectUpdateForm form);
}