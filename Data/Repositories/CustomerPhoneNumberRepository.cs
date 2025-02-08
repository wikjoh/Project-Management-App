using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class CustomerPhoneNumberRepository(DataContext context) : BaseRepository<CustomerPhoneNumberEntity>(context), ICustomerPhoneNumberRepository
{
    private readonly DataContext _context = context;
}
