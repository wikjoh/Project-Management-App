using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class ServiceFactory
{
    public static ServiceRegistrationForm Create()
    {
        return new ServiceRegistrationForm();
    }



    public static ServiceEntity Create(ServiceRegistrationForm form)
    {
        return new ServiceEntity
        {
            Name = form.Name,
            Price = form.Price,
            UnitId = form.UnitId,
        };
    }



    public static ServiceModel Create(ServiceEntity entity)
    {
        return new ServiceModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            UnitId = entity.UnitId,
            Unit = entity.Unit.Unit
        };
    }
}
