using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IServiceUnitService
{
    Task<IServiceResult> CreateServiceUnitAsync(ServiceUnitRegistrationForm form);
    Task<IServiceResult> DeleteServiceUnitAsync(ServiceUnitModel model);
    Task<IServiceResult> GetAllServiceUnitsAsync();
    Task<IServiceResult> GetServiceUnitByIdAsync(int id);
    Task<IServiceResult> UpdateServiceUnitAsync(ServiceUnitUpdateForm form);
}