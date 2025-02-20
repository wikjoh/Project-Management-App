using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IServiceService
{
    Task<IServiceResult> CreateServiceAsync(ServiceRegistrationForm form);
    Task<IServiceResult> DeleteServiceAsync(ServiceModel model);
    Task<IServiceResult> GetAllServicesWithUnitAsync();
    Task<IServiceResult> GetServiceByIdWithUnitAsync(int id);
    Task<IServiceResult> UpdateServiceAsync(ServiceUpdateForm form);
}