using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserRegistrationForm Create()
    {
        return new UserRegistrationForm();
    }


    public static UserEntity ToEntity(UserRegistrationForm form)
    {
        return new UserEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            EmailAddress = form.EmailAddress,
        };
    }


    public static UserModel ToModel(UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DisplayName = entity.DisplayName,
            EmailAddress = entity.EmailAddress,
        };
    }

    public static UserModelDetailed ToModelDetailed(UserEntity entity)
    {
        return new UserModelDetailed
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DisplayName = entity.DisplayName,
            EmailAddress = entity.EmailAddress,
            Roles = entity.UserRoles?.Select(ur => RoleFactory.ToModel(ur.Role)).ToList() ?? [],
            Projects = entity.Projects?.Select(p => ProjectFactory.ToModel(p)).ToList() ?? []
        };
    }
}
