using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface ICustomerService
{
    Task<bool> CreateCustomerAsync(CustomerRegistrationForm form);
    Task<bool> DeleteCustomerByEmailAsync(string email);
    Task<bool> DeleteCustomerByIdAsync(int id);
    Task<IEnumerable<CustomerModel>?> GetAllCustomersAsync();
    Task<CustomerModel?> GetCustomerByEmailAsync(string emailAddress);
    Task<CustomerModel?> GetCustomerByIdAsync(int id);
    Task<CustomerModel?> UpdateCustomerAsync(CustomerUpdateForm form);
}