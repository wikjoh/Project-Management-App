using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi;

internal class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=192.168.200.50;Database=project_mgmt;User ID=project_mgmt;Password=BytMig123!;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<ICustomerPhoneNumberRepository, CustomerPhoneNumberRepository>();
        builder.Services.AddScoped<ICustomerPhoneNumberService, CustomerPhoneNumberService>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddScoped<IUserRoleService, UserRoleService>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IServiceUnitRepository, ServiceUnitRepository>();
        builder.Services.AddScoped<IServiceUnitService, ServiceUnitService>();
        builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
        builder.Services.AddScoped<IServiceService, ServiceService>();

        var app = builder.Build();

        app.MapOpenApi();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.MapControllers();
        app.Run();
    }
}