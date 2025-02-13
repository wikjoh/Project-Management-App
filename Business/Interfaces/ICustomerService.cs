using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface ICustomerService
{
    Task<bool> CreateCustomerAsync(CustomerRegistrationForm form);
    Task<bool> DeleteCustomerByEmailAsync(string email);
    Task<bool> DeleteCustomerByIdAsync(int id);
    Task<IEnumerable<CustomerModel>?> GetAllWithPhoneAsync();
    Task<CustomerModel?> GetByEmailWithPhoneAsync(string emailAddress);
    Task<CustomerModel?> GetByIdWithPhoneAsync(int id);
    Task<CustomerModel?> UpdateCustomerAsync(CustomerUpdateForm form);
}