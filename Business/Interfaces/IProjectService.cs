using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IProjectService
{
    Task<IServiceResult> CreateProject(ProjectRegistrationForm form);
    Task<IServiceResult> DeleteProjectById(int id);
    Task<IServiceResult> GetAllProjects();
    Task<IServiceResult> GetAllProjectsDetailed();
    Task<IServiceResult> GetProjectById(int id);
    Task<IServiceResult> GetProjectByIdDetailed(int id);
    Task<IServiceResult> UpdateProject(ProjectUpdateForm form);
}