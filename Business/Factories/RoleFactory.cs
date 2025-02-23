using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class RoleFactory
{
    public static RoleRegistrationForm Create()
    {
        return new RoleRegistrationForm();
    }


    public static RoleEntity ToEntity(RoleRegistrationForm form)
    {
        return new RoleEntity
        {
            Role = form.Role,
        };
    }


    public static RoleModel ToModel(RoleEntity entity)
    {
        return new RoleModel
        {
            Id = entity.Id,
            Role = entity.Role,
        };
    }


    public static RoleModelDetailed ToModelDetailed(RoleEntity entity)
    {
        return new RoleModelDetailed
        {
            Id = entity.Id,
            Role = entity.Role,
            Users = entity.UserRoles?.Select(ur => UserFactory.ToModel(ur.User)).ToList() ?? []
        };
    }
}
