using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Diagnostics;

namespace Business.Services;

public class CustomerPhoneNumberService(ICustomerPhoneNumberRepository customerPhoneNumberRepository) : ICustomerPhoneNumberService
{
    private readonly ICustomerPhoneNumberRepository _customerPhoneNumberRepository = customerPhoneNumberRepository;


    // CREATE
    public async Task<bool> AddPhoneNumberAsync(CustomerPhoneNumberForm form)
    {
        bool? phoneNumberExists = await _customerPhoneNumberRepository.ExistsAsync(x => x.PhoneNumber == form.PhoneNumber);
        if (phoneNumberExists == true || form == null)
            return false;

        try
        {
            var entity = CustomerPhoneNumberFactory.Create(form);

            await _customerPhoneNumberRepository.CreateAsync(entity);
            await _customerPhoneNumberRepository.SaveAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed to add phone number. {ex.Message}");
            return false;
        }
    }


    // READ
    public async Task<IEnumerable<CustomerPhoneNumberModel>> GetAllPhoneNumbersByCustomerIdAsync(int id)
    {
        var phoneNumbers = await _customerPhoneNumberRepository.GetAllWhereAsync(x => x.CustomerId == id);
        return phoneNumbers != null ? phoneNumbers.Select(x => CustomerPhoneNumberFactory.Create(x)) : [];
    }


    // UPDATE
    public async Task<CustomerPhoneNumberModel?> UpdatePhoneNumberAsync(CustomerPhoneNumberForm form)
    {
        var existing = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
        if (existing == null || form == null)
            return null;

        existing.IsWorkNumber = form.IsWorkNumber;
        existing.IsCellNumber = form.IsCellNumber;
        existing.IsHomeNumber = form.IsHomeNumber;

        try
        {
            _customerPhoneNumberRepository.Update(existing);
            await _customerPhoneNumberRepository.SaveAsync();

            var updated = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == form.PhoneNumber && x.CustomerId == form.CustomerId);
            return CustomerPhoneNumberFactory.Create(updated!);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed updating phone number. {ex.Message}");
            return null;
        }

    }


    // DELETE
    public async Task<bool> DeletePhoneNumber(CustomerPhoneNumberModel model)
    {
        var phoneEntity = await _customerPhoneNumberRepository.GetOneAsync(x => x.PhoneNumber == model.PhoneNumber && x.CustomerId == model.CustomerId);
        if (phoneEntity == null || model == null)
            return false;

        try
        {
            _customerPhoneNumberRepository.Delete(phoneEntity);
            var result = await _customerPhoneNumberRepository.SaveAsync();
            if (result == 1)
                return true;
            else
                return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed deleting phone number. {ex.Message}");
            return false;
        }
    }
}
