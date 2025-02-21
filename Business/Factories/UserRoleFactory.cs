using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class UserRoleFactory
{
    public static UserRoleEntity Create(int userId, int roleId)
    {
        return new UserRoleEntity
        {
            UserId = userId,
            RoleId = roleId
        };
    }


    public static UserRoleModel ToModel(UserRoleEntity entity)
    {
        return new UserRoleModel
        {
            UserId = entity.UserId,
            RoleId = entity.RoleId
        };
    }
}
