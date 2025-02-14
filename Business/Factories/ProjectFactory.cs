using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectRegistrationForm Create()
    {
        return new ProjectRegistrationForm();
    }


    public static ProjectEntity Create(ProjectRegistrationForm form)
    {
        return new ProjectEntity
        {
            Name = form.Name,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            ProjectManagerId = form.ProjectManagerId,
            CustomerName = form.CustomerName,
            CustomerId = form.CustomerId,
            ServiceId = form.ServiceId,
            ServiceQuantity = form.ServiceQuantity,
            TotalPrice = form.TotalPrice,
            StatusId = form.StatusId,
        };
    }


    public static ProjectModel Create(ProjectEntity entity)
    {
        return new ProjectModel
        {
            Id = entity.Id,
            Name = entity.Name,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            ProjectManagerId = entity.ProjectManagerId,
            CustomerName = entity.CustomerName,
            CustomerId = entity.CustomerId,
            ServiceId = entity.ServiceId,
            ServiceQuantity = entity.ServiceQuantity,
            TotalPrice = entity.TotalPrice,
            StatusId = entity.StatusId,
        };
    }
}
