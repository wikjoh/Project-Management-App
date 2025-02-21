using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface ICustomerService
{
    Task<IServiceResult> CreateCustomerAsync(CustomerRegistrationForm form);
    Task<IServiceResult> DeleteCustomerByEmailAsync(string email);
    Task<IServiceResult> DeleteCustomerByIdAsync(int id);
    Task<IServiceResult> GetAllAsync();
    Task<IServiceResult> GetAllDetailedAsync();
    Task<IServiceResult> GetByEmailAsync(string emailAddress);
    Task<IServiceResult> GetByEmailDetailedAsync(string emailAddress);
    Task<IServiceResult> GetByIdAsync(int id);
    Task<IServiceResult> GetByIdDetailedAsync(int id);
    Task<IServiceResult> UpdateCustomerAsync(CustomerUpdateForm form);
}