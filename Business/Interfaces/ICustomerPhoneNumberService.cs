using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface ICustomerPhoneNumberService
{
    Task<bool> AddPhoneNumberAsync(CustomerPhoneNumberForm form);
    Task<bool> DeletePhoneNumber(CustomerPhoneNumberModel model);
    Task<IEnumerable<CustomerPhoneNumberModel>> GetAllPhoneNumbersByCustomerIdAsync(int id);
    Task<CustomerPhoneNumberModel?> UpdatePhoneNumberAsync(CustomerPhoneNumberForm form);
}