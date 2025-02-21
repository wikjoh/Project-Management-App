using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
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

            form.PhoneNumberForm.CustomerId = customerEntity.Id;
            var phoneNumberResult = await _customerPhoneNumberService.AddPhoneNumberAsync(form.PhoneNumberForm);
            if (!phoneNumberResult.Success) throw new Exception("Failed to create phone number entity.");

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
    public async Task<IServiceResult> GetAllWithPhoneAsync()
    {
        var customers = await _customerRepository.GetAllAsync(q => q.Include(c => c.PhoneNumbers));
        customers ??= [];

        var customerListWithPhone = ServiceResult<IEnumerable<CustomerModelDetailed>?>.Ok(customers.Select(x => CustomerFactory.ToModelDetailed(x)));
        return customerListWithPhone;
    }

    public async Task<IServiceResult> GetByIdWithPhoneAsync(int id)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.Id == id, q => q.Include(c => c.PhoneNumbers));

        if (customerEntity == null)
            return ServiceResult.NotFound($"Customer with id {id} not found.");

        var customerWithPhone = CustomerFactory.ToModelDetailed(customerEntity);
        return ServiceResult<CustomerModelDetailed>.Ok(customerWithPhone);
    }

    public async Task<IServiceResult> GetByEmailWithPhoneAsync(string emailAddress)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.EmailAddress == emailAddress, q => q.Include(c => c.PhoneNumbers));

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

        var customer = await _customerRepository.GetOneAsync(x => x.Id == form.Id);

        if (customer == null)
            return ServiceResult.NotFound($"Customer not found.");

        customer.IsCompany = form.IsCompany;
        customer.FirstName = form.FirstName;
        customer.LastName = form.LastName;
        customer.CompanyName = form.CompanyName;
        customer.EmailAddress = form.EmailAddress;

        _customerRepository.Update(customer);
        var result = await _customerRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Updating customer failed.");

        customer = await _customerRepository.GetOneAsync(x => x.Id == form.Id);
        return ServiceResult<CustomerModelDetailed?>.Ok(customer != null
            ? CustomerFactory.ToModelDetailed(customer)
            : null);
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
