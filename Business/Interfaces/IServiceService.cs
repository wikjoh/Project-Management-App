using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IServiceService
{
    Task<IServiceResult> CreateServiceAsync(ServiceRegistrationForm form);
    Task<IServiceResult> DeleteServiceAsync(ServiceModel model);
    Task<IServiceResult> GetAllServicesAsync();
    Task<IServiceResult> GetAllServicesDetailedAsync();
    Task<IServiceResult> GetServiceByIdDetailedAsync(int id);
    Task<IServiceResult> UpdateServiceAsync(ServiceUpdateForm form);
}