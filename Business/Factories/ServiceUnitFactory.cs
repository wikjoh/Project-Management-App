﻿using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ServiceUnitFactory
{
    public static ServiceUnitRegistrationForm Create()
    {
        return new ServiceUnitRegistrationForm();
    }


    public static ServiceUnitEntity ToEntity(ServiceUnitRegistrationForm form)
    {
        return new ServiceUnitEntity
        {
            Unit = form.Unit,
        };
    }


    public static ServiceUnitModel ToModel(ServiceUnitEntity entity)
    {
        return new ServiceUnitModel
        {
            Id = entity.Id,
            Unit = entity.Unit,
        };
    }


    public static ServiceUnitModelDetailed ToModelDetailed(ServiceUnitEntity entity)
    {
        return new ServiceUnitModelDetailed
        {
            Id = entity.Id,
            Unit = entity.Unit,
            Services = entity.Services?.Select(s => ServiceFactory.ToModel(s)).ToList() ?? []
        };
    }
}
