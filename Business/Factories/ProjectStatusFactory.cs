﻿using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectStatusFactory
{
    public static ProjectStatusRegistrationForm Create()
    {
        return new ProjectStatusRegistrationForm();
    }


    public static ProjectStatusEntity ToEntity(ProjectStatusRegistrationForm entity)
    {
        return new ProjectStatusEntity
        {
            Name = entity.Name,
        };
    }


    public static ProjectStatusModel ToModel(ProjectStatusEntity entity)
    {
        return new ProjectStatusModel
        {
            Id = entity.Id,
            Name = entity.Name,
        };
    }
}
