using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class CustomerService(ICustomerRepository customerRepository, ICustomerPhoneNumberService customerPhoneNumberService) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerPhoneNumberService _customerPhoneNumberService = customerPhoneNumberService;


    // CREATE
    public async Task<bool> CreateCustomerAsync(CustomerRegistrationForm form)
    {
        bool? customerExists = await _customerRepository.ExistsAsync(x => x.EmailAddress == form.EmailAddress);
        if (customerExists == true)
            return false;

        var customerEntity = CustomerFactory.Create(form);

        await _customerRepository.BeginTransactionAsync();

        try
        {
            await _customerRepository.CreateAsync(customerEntity);
            var customerResult = (await _customerRepository.SaveAsync() == 1) ? true : false;
            if (!customerResult) throw new Exception("Failed to create customer entity.");

            form.PhoneNumberForm.CustomerId = customerEntity.Id;
            var phoneNumberResult = await _customerPhoneNumberService.AddPhoneNumberAsync(form.PhoneNumberForm);
            if (!phoneNumberResult) throw new Exception("Failed to create phone number entity.");

            await _customerRepository.CommitTransactionAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Failed creating customer. {ex.Message}");
            await _customerRepository.RollbackTransactionAsync();
            return false;
        }

    }


    // READ
    public async Task<IEnumerable<CustomerModel>?> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        return customers != null
            ? customers.Select(x => CustomerFactory.Create(x))
            : [];
    }

    public async Task<CustomerModel?> GetCustomerByIdAsync(int id)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.Id == id);
        return customerEntity != null ? CustomerFactory.Create(customerEntity) : null;
    }

    public async Task<CustomerModel?> GetCustomerByEmailAsync(string emailAddress)
    {
        var customerEntity = await _customerRepository.GetOneAsync(x => x.EmailAddress == emailAddress);
        return customerEntity != null ? CustomerFactory.Create(customerEntity) : null;
    }


    // UPDATE
    public async Task<CustomerModel?> UpdateCustomerAsync(CustomerUpdateForm form)
    {
        var customer = await _customerRepository.GetOneAsync(x => x.Id == form.Id);
        if (customer == null)
            return null;

        customer.IsCompany = form.IsCompany;
        customer.FirstName = form.FirstName;
        customer.LastName = form.LastName;
        customer.CompanyName = form.CompanyName;
        customer.EmailAddress = form.EmailAddress;

        _customerRepository.Update(customer);
        await _customerRepository.SaveAsync();

        customer = await _customerRepository.GetOneAsync(x => x.Id == form.Id);
        return customer != null
            ? CustomerFactory.Create(customer)
            : null;
    }



    // DELETE
    public async Task<bool> DeleteCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetOneAsync(x => x.Id == id);
        if (customer == null)
            return false;

        _customerRepository.Delete(customer);
        var result = await _customerRepository.SaveAsync();
        return result == 1;
    }

    public async Task<bool> DeleteCustomerByEmailAsync(string email)
    {
        var customer = await _customerRepository.GetOneAsync(x => x.EmailAddress == email);
        if (customer == null)
            return false;

        _customerRepository.Delete(customer);
        var result = await _customerRepository.SaveAsync();
        return result == 1;
    }
}
