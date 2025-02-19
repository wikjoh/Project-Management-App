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


    public static UserEntity Create(UserRegistrationForm form)
    {
        return new UserEntity
        {
            FirstName = form.FirstName,
            LastName = form.LastName,
            EmailAddress = form.EmailAddress,
        };
    }


    public static UserModel Create(UserEntity entity)
    {
        return new UserModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DisplayName = entity.DisplayName,
            EmailAddress = entity.EmailAddress,
            Roles = entity.UserRoles?.Select(ur => RoleFactory.Create(ur.Role)).ToList() ?? []
        };
    }
}
