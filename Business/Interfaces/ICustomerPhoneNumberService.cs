﻿using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface ICustomerPhoneNumberService
{
    Task<IServiceResult> AddPhoneNumberAsync(CustomerPhoneNumberForm form);
    Task<IServiceResult> DeletePhoneNumber(CustomerPhoneNumberModel model);
    Task<IServiceResult> GetAllPhoneNumbersByCustomerIdAsync(int id);
    Task<IServiceResult> UpdatePhoneNumberAsync(CustomerPhoneNumberForm form);
}