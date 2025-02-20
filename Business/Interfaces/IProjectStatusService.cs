using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IProjectStatusService
{
    Task<IServiceResult> CreateProjectStatusAsync(ProjectStatusRegistrationForm form);
    Task<IServiceResult> DeleteProjectStatusAsync(ProjectStatusModel model);
    Task<IServiceResult> GetAllProjectStatuses();
    Task<IServiceResult> GetProjectStatusByIdAsync(int id);
    Task<IServiceResult> UpdateProjectStatusAsync(ProjectStatusUpdateForm form);
}