using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;

namespace Business.Services;

public class CustomerPhoneNumberService(ICustomerPhoneNumberRepository customerPhoneNumberRepository) : ICustomerPhoneNumberService
{
    private readonly ICustomerPhoneNumberRepository _customerPhoneNumberRepository = customerPhoneNumberRepository;


    // CREATE
    public async Task<IServiceResult> AddPhoneNumberAsync(CustomerPhoneNumberForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        bool? phoneNumberExists = await _customerPhoneNumberRepository.ExistsAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
        if (phoneNumberExists == true)
            return ServiceResult.AlreadyExists($"Phone number {form.PhoneNumber} already exists on customer {form.CustomerId}.");
        if (phoneNumberExists == null)
            return ServiceResult.InternalServerError("Failed verifying if phone number exists");

        var entity = CustomerPhoneNumberFactory.Create(form);

        await _customerPhoneNumberRepository.CreateAsync(entity);
        var result = await _customerPhoneNumberRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating customer phone number.");

        var phoneNumber = CustomerPhoneNumberFactory.Create(entity);
        return ServiceResult<CustomerPhoneNumberModel>.Ok(phoneNumber);
    }


    // READ
    public async Task<IServiceResult> GetAllPhoneNumbersByCustomerIdAsync(int id)
    {
        var phoneNumbers = await _customerPhoneNumberRepository.GetAllWhereAsync(x => x.CustomerId == id);
        var phoneNumbersList = phoneNumbers != null ? phoneNumbers.Select(x => CustomerPhoneNumberFactory.Create(x)) : [];

        return ServiceResult<IEnumerable<CustomerPhoneNumberModel>>.Ok(phoneNumbersList);
    }


    // UPDATE
    public async Task<IServiceResult> UpdatePhoneNumberAsync(CustomerPhoneNumberForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var existing = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
        if (existing == null)
            return ServiceResult.NotFound($"Phone number {form.PhoneNumber} not found on customerId {form.CustomerId}");

        existing.IsWorkNumber = form.IsWorkNumber;
        existing.IsCellNumber = form.IsCellNumber;
        existing.IsHomeNumber = form.IsHomeNumber;


        _customerPhoneNumberRepository.Update(existing);
        var result = await _customerPhoneNumberRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed updating phone number.");

        var updated = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
        return ServiceResult<CustomerPhoneNumberModel?>.Ok(CustomerPhoneNumberFactory.Create(updated!));
    }


    // DELETE
    public async Task<IServiceResult> DeletePhoneNumberAsync(CustomerPhoneNumberModel model)
    {
        if (model == null)
            return ServiceResult.BadRequest("Parameter cannot be empty.");

        var phoneEntity = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == model.PhoneNumber && x.CustomerId == model.CustomerId);
        if (phoneEntity == null)
            return ServiceResult.NotFound("Phone number not found.");


        _customerPhoneNumberRepository.Delete(phoneEntity);
        var result = await _customerPhoneNumberRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting phone number failed.");

        return ServiceResult.Ok();

    }
}
