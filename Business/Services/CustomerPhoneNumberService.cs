using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Diagnostics;

namespace Business.Services;

public class CustomerPhoneNumberService(ICustomerPhoneNumberRepository customerPhoneNumberRepository) : ICustomerPhoneNumberService
{
    private readonly ICustomerPhoneNumberRepository _customerPhoneNumberRepository = customerPhoneNumberRepository;


    // CREATE
    public async Task<IServiceResult> AddPhoneNumberAsync(CustomerPhoneNumberRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        bool? phoneNumberExists = await _customerPhoneNumberRepository.ExistsAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
        if (phoneNumberExists == true)
            return ServiceResult.AlreadyExists($"Phone number {form.PhoneNumber} already exists on customer {form.CustomerId}.");
        if (phoneNumberExists == null)
            return ServiceResult.InternalServerError("Failed verifying if phone number exists");

        var entity = CustomerPhoneNumberFactory.ToEntity(form);

        await _customerPhoneNumberRepository.CreateAsync(entity);
        var result = await _customerPhoneNumberRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating customer phone number.");

        var phoneNumber = CustomerPhoneNumberFactory.ToModel(entity);
        return ServiceResult<CustomerPhoneNumberModel>.Ok(phoneNumber);
    }


    // READ
    public async Task<IServiceResult> GetAllPhoneNumbersByCustomerIdAsync(int id)
    {
        var phoneNumbers = await _customerPhoneNumberRepository.GetAllWhereAsync(x => x.CustomerId == id);
        var phoneNumbersList = phoneNumbers != null ? phoneNumbers.Select(x => CustomerPhoneNumberFactory.ToModel(x)) : [];

        return ServiceResult<IEnumerable<CustomerPhoneNumberModel>>.Ok(phoneNumbersList);
    }


    // UPDATE
    public async Task<IServiceResult> UpdatePhoneNumberAsync(CustomerPhoneNumberUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var existingNumber = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == form.ExistingPhoneNumber && x.CustomerId == form.CustomerId);
        if (existingNumber == null)
            return ServiceResult.NotFound($"Phone number {form.ExistingPhoneNumber} not found on customerId {form.CustomerId}");

        try
        {
            if (form.ExistingPhoneNumber == form.PhoneNumber)
            {
                existingNumber.IsHomeNumber = form.IsHomeNumber;
                existingNumber.IsWorkNumber = form.IsWorkNumber;
                existingNumber.IsCellNumber = form.IsCellNumber;

                _customerPhoneNumberRepository.Update(existingNumber);
                var updateResult = await _customerPhoneNumberRepository.SaveAsync() > 0;
                if (!updateResult) throw new Exception($"Failed setting new phone number types on existing phone number {existingNumber.PhoneNumber} on customerId {existingNumber.CustomerId}");
                
                var updatedNumber = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
                if (updatedNumber == null) throw new Exception($"Returned null entity after update.");

                return ServiceResult<CustomerPhoneNumberModel>.Ok(CustomerPhoneNumberFactory.ToModel(updatedNumber));
            }
            else
            {
                var newNumberEntity = CustomerPhoneNumberFactory.ToEntity(form);

                await _customerPhoneNumberRepository.CreateAsync(newNumberEntity);
                var addResult = await _customerPhoneNumberRepository.SaveAsync() > 0;
                if (!addResult) throw new Exception($"Failed to add number {form.PhoneNumber}.");

                _customerPhoneNumberRepository.Delete(existingNumber);
                var deleteResult = await _customerPhoneNumberRepository.SaveAsync() > 0;
                if (!deleteResult) throw new Exception($"Failed to delete existing number {form.ExistingPhoneNumber}");

                newNumberEntity = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
                if (newNumberEntity == null) throw new Exception($"Retrieved null entity after update.");

                var newNumber = CustomerPhoneNumberFactory.ToModel(newNumberEntity);
                return ServiceResult<CustomerPhoneNumberModel>.Ok(newNumber);
            }
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed updating phone number {form.ExistingPhoneNumber} on customerId {form.CustomerId}. {ex.Message}";
            Debug.WriteLine(errorMessage);
            // Throw exception to executing parent function which is running the transaction and will roll it back
            throw new Exception(errorMessage);
        }
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
