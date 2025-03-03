﻿using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public class CustomerPhoneNumberFactory
{
    public static CustomerPhoneNumberRegistrationForm Create()
    {
        return new CustomerPhoneNumberRegistrationForm();
    }


    public static CustomerPhoneNumberEntity ToEntity(CustomerPhoneNumberRegistrationForm form)
    {
        return new CustomerPhoneNumberEntity
        {
            CustomerId = form.CustomerId,
            PhoneNumber = form.PhoneNumber,
            IsWorkNumber = form.IsWorkNumber,
            IsCellNumber = form.IsCellNumber,
            IsHomeNumber = form.IsHomeNumber,
        };
    }

    public static CustomerPhoneNumberEntity ToEntity(CustomerPhoneNumberUpdateForm form)
    {
        return new CustomerPhoneNumberEntity
        {
            CustomerId = form.CustomerId,
            PhoneNumber = form.PhoneNumber,
            IsWorkNumber = form.IsWorkNumber,
            IsCellNumber = form.IsCellNumber,
            IsHomeNumber = form.IsHomeNumber,
        };
    }


    public static CustomerPhoneNumberModel ToModel(CustomerPhoneNumberEntity entity)
    {
        return new CustomerPhoneNumberModel
        {
            CustomerId = entity.CustomerId,
            PhoneNumber = entity.PhoneNumber,
            IsWorkNumber = entity.IsWorkNumber,
            IsCellNumber = entity.IsCellNumber,
            IsHomeNumber = entity.IsHomeNumber,
        };
    }
}
