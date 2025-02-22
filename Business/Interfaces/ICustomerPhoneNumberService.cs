using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface ICustomerPhoneNumberService
{
    Task<IServiceResult> AddPhoneNumberAsync(CustomerPhoneNumberRegistrationForm form);
    Task<IServiceResult> DeletePhoneNumberAsync(CustomerPhoneNumberModel model);
    Task<IServiceResult> GetAllPhoneNumbersByCustomerIdAsync(int id);
    Task<IServiceResult> UpdatePhoneNumberAsync(CustomerPhoneNumberUpdateForm form);
}