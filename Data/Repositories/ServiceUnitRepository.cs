using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ServiceUnitRepository(DataContext context) : BaseRepository<ServiceUnitEntity>(context), IServiceUnitRepository
{
    
}
