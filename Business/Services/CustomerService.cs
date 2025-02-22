using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Business.Services;

public class CustomerService(ICustomerRepository customerRepository, ICustomerPhoneNumberService customerPhoneNumberService) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerPhoneNumberService _customerPhoneNumberService = customerPhoneNumberService;


    // CREATE
    public async Task<IServiceResult> CreateCustomerAsync(CustomerRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Invalid registration form.");

        bool? customerExists = await _customerRepository.ExistsAsync(x => x.EmailAddress == form.EmailAddress);
        if (customerExists == true)
            return ServiceResult.AlreadyExists("Customer with given email address already exists.");

        var customerEntity = CustomerFactory.ToEntity(form);

        await _customerRepository.BeginTransactionAsync();

        try
        {
            await _customerRepository.CreateAsync(customerEntity);
            var customerResult = await _customerRepository.SaveAsync() > 0;
            if (!customerResult) throw new Exception("Failed to create customer entity.");

            if (form.PhoneNumbers != null)
            {
                foreach (var phoneNumber in form.PhoneNumbers)
                {
                    phoneNumber.CustomerId = customerEntity.Id;
                    var phoneNumberResult = await _customerPhoneNumberService.AddPhoneNumberAsync(phoneNumber);
                    if (!phoneNumberResult.Success) throw new Exception("Failed to create phone number entity.");
                }
            }

            await _customerRepository.CommitTransactionAsync();
            var createdCustomerWithPhoneNumber = CustomerFactory.ToModelDetailed((await _customerRepository.GetOneAsync(x => x.Id == customerEntity.Id, q => q.Include(c => c.PhoneNumbers)))!);
            return ServiceResult<CustomerModelDetailed>.Created(createdCustomerWithPhoneNumber);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed creating customer. Rolling back. {ex.Message}";
            Debug.WriteLine(errorMessage);
            await _customerRepository.RollbackTransactionAsync();
            return ServiceResult.InternalServerError(errorMessage);
        }
    }


    // READ
    public async Task<IServiceResult> GetAllAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        customers ??= [];

        var customerListWithPhone = ServiceResult<IEnumerable<CustomerModel>?>.Ok(customers.Select(x => CustomerFactory.ToModel(x)));
        return customerListWithPhone;
    }

    public async Task<IServiceResult> GetAllDetailedAsync()
    {
        var customers = await _customerRepository.GetAllAsync(q => q.Include(c => c.PhoneNumbers).Include(c => c.Projects));
        customers ??= [];

        var customerListWithPhone = ServiceResult<IEnumerable<CustomerModelDetailed>?>.Ok(customers.Select(x => CustomerFactory.ToModelDetailed(x)));
        return customerListWithPhone;
    }

    public async Task<IServiceResult> GetByIdAsync(int id)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.Id == id);

        if (customerEntity == null)
            return ServiceResult.NotFound($"Customer with id {id} not found.");

        var customerWithPhone = CustomerFactory.ToModel(customerEntity);
        return ServiceResult<CustomerModel>.Ok(customerWithPhone);
    }

    public async Task<IServiceResult> GetByIdDetailedAsync(int id)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.Id == id, q => q.Include(c => c.PhoneNumbers).Include(c => c.Projects));

        if (customerEntity == null)
            return ServiceResult.NotFound($"Customer with id {id} not found.");

        var customerWithPhone = CustomerFactory.ToModelDetailed(customerEntity);
        return ServiceResult<CustomerModelDetailed>.Ok(customerWithPhone);
    }

    public async Task<IServiceResult> GetByEmailAsync(string emailAddress)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.EmailAddress == emailAddress);

        if (customerEntity == null)
            return ServiceResult.NotFound($"Customer with email address {emailAddress} not found.");

        var customerWithPhone = CustomerFactory.ToModel(customerEntity);
        return ServiceResult<CustomerModel>.Ok(customerWithPhone);
    }

    public async Task<IServiceResult> GetByEmailDetailedAsync(string emailAddress)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.EmailAddress == emailAddress, q => q.Include(c => c.PhoneNumbers).Include(c => c.Projects));

        if (customerEntity == null)
            return ServiceResult.NotFound($"Customer with email address {emailAddress} not found.");

        var customerWithPhone = CustomerFactory.ToModelDetailed(customerEntity);
        return ServiceResult<CustomerModelDetailed>.Ok(customerWithPhone);
    }


    // UPDATE
    public async Task<IServiceResult> UpdateCustomerAsync(CustomerUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var customer = await _customerRepository.GetOneAsync(x => x.Id == form.Id, q => q.Include(c => c.PhoneNumbers));

        if (customer == null)
            return ServiceResult.NotFound($"Customer not found.");

        await _customerRepository.BeginTransactionAsync();

        try
        {
            customer.IsCompany = form.IsCompany;
            customer.FirstName = form.FirstName;
            customer.LastName = form.LastName;
            customer.CompanyName = form.CompanyName;
            customer.EmailAddress = form.EmailAddress;

            _customerRepository.Update(customer);
            var result = await _customerRepository.SaveAsync() > 0;
            if (!result)
                return ServiceResult.InternalServerError("Updating customer failed.");

            var cpnIterationCopy = customer.PhoneNumbers.ToList(); // prevent "Collection was modified" exception due to entity being modified during loop
            if (form.PhoneNumbers != null)
            {
                foreach (var phoneNumber in form.PhoneNumbers)
                {
                    // If ExistingPN is null in form, add phone number
                    if (phoneNumber.ExistingPhoneNumber == null && phoneNumber.PhoneNumber != null)
                    {
                        var addResult = await _customerPhoneNumberService.AddPhoneNumberAsync(new CustomerPhoneNumberRegistrationForm()
                        {
                            CustomerId = phoneNumber.CustomerId,
                            PhoneNumber = phoneNumber.PhoneNumber,
                            IsWorkNumber = phoneNumber.IsWorkNumber,
                            IsHomeNumber = phoneNumber.IsHomeNumber,
                            IsCellNumber = phoneNumber.IsCellNumber
                        });
                        if (!addResult.Success) throw new Exception($"Failed to add new number {phoneNumber} on customerId {phoneNumber.CustomerId}.");
                    }
                    // If ExistingPN is NOT null AND ExistingPN does NOT equal PN, update phone number
                    else if (phoneNumber.ExistingPhoneNumber != null)
                    {
                        var updateResult = await _customerPhoneNumberService.UpdatePhoneNumberAsync(phoneNumber);
                        if (!updateResult.Success) throw new Exception("Failed to update phone number entity.");
                    }
                }
            }

            await _customerRepository.CommitTransactionAsync();
            var createdCustomerWithPhoneNumber = CustomerFactory.ToModelDetailed((await _customerRepository.GetOneAsync(x => x.Id == form.Id, q => q.Include(c => c.PhoneNumbers)))!);
            return ServiceResult<CustomerModelDetailed>.Created(createdCustomerWithPhoneNumber);
        }
        catch (Exception ex)
        {
            string errorMessage = $"Failed updating customer. Rolling back. {ex.Message}";
            Debug.WriteLine(errorMessage);
            await _customerRepository.RollbackTransactionAsync();
            return ServiceResult.InternalServerError(errorMessage);
        }
    }



    // DELETE
    public async Task<IServiceResult> DeleteCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetOneAsync(x => x.Id == id);
        if (customer == null)
            return ServiceResult.NotFound($"Customer with id {id} not found.");

        _customerRepository.Delete(customer);
        var result = await _customerRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting customer failed.");

        return ServiceResult.Ok();
    }

    public async Task<IServiceResult> DeleteCustomerByEmailAsync(string emailAddress)
    {
        var customer = await _customerRepository.GetOneAsync(x => x.EmailAddress == emailAddress);
        if (customer == null)
            return ServiceResult.NotFound($"Customer with email address {emailAddress} not found.");

        _customerRepository.Delete(customer);
        var result = await _customerRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting customer failed.");

        return ServiceResult.Ok();
    }
}
