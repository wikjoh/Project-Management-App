using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class CustomerFactory
{
    public static CustomerRegistrationForm Create()
    {
        return new CustomerRegistrationForm();
    }


    public static CustomerEntity Create(CustomerRegistrationForm form)
    {
            return new CustomerEntity
            {
                IsCompany = form.IsCompany,
                FirstName = form.FirstName,
                LastName = form.LastName,
                CompanyName = form.CompanyName,
                EmailAddress = form.EmailAddress,
            };
    }


    public static CustomerModel Create(CustomerEntity entity)
    {
        return new CustomerModel
        {
            Id = entity.Id,
            IsCompany = entity.IsCompany,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            CompanyName = entity.CompanyName,
            EmailAddress = entity.EmailAddress,
            PhoneNumbers = entity.PhoneNumbers?.Select(pn => CustomerPhoneNumberFactory.Create(pn)).ToList() ?? []
        };
    }
}
