using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface ICustomerService
{
    Task<IServiceResult> CreateCustomerAsync(CustomerRegistrationForm form);
    Task<IServiceResult> DeleteCustomerByEmailAsync(string email);
    Task<IServiceResult> DeleteCustomerByIdAsync(int id);
    Task<IServiceResult> GetAllWithPhoneAsync();
    Task<IServiceResult> GetByEmailWithPhoneAsync(string emailAddress);
    Task<IServiceResult> GetByIdWithPhoneAsync(int id);
    Task<IServiceResult> UpdateCustomerAsync(CustomerUpdateForm form);
}