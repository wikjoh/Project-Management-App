using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class UserRoleRepository(DataContext context) : BaseRepository<UserRoleEntity>(context), IUserRoleRepository
{

}
