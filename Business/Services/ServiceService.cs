using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class ServiceService(IServiceRepository serviceRepository) : IServiceService
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;


    // CREATE
    public async Task<IServiceResult> CreateServiceAsync(ServiceRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Invalid registration form.");

        bool? serviceExists = await _serviceRepository.ExistsAsync(x => x.Name == form.Name);
        if (serviceExists == true)
            return ServiceResult.AlreadyExists($"Service {form.Name} already exists.");

        var serviceEntity = ServiceFactory.ToEntity(form);

        await _serviceRepository.CreateAsync(serviceEntity);
        var result = await _serviceRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating service.");

        var serviceEntityWithUnit = await _serviceRepository.GetOneAsync(x => x.Id == serviceEntity.Id, q => q.Include(s => s.Unit));
        return ServiceResult<ServiceModelDetailed>.Ok(ServiceFactory.ToModelDetailed(serviceEntityWithUnit!));
    }


    // READ
    public async Task<IServiceResult> GetAllServicesAsync()
    {
        var serviceEntities = await _serviceRepository.GetAllAsync();
        var serviceList = serviceEntities != null ? serviceEntities.Select(x => ServiceFactory.ToModel(x)) : [];

        return ServiceResult<IEnumerable<ServiceModel>>.Ok(serviceList);
    }

    public async Task<IServiceResult> GetAllServicesDetailedAsync()
    {
        var serviceEntities = await _serviceRepository.GetAllAsync(q => q.Include(s => s.Unit).Include(s => s.Projects));
        var serviceList = serviceEntities != null ? serviceEntities.Select(x => ServiceFactory.ToModelDetailed(x)) : [];

        return ServiceResult<IEnumerable<ServiceModelDetailed>>.Ok(serviceList);
    }


    public async Task<IServiceResult> GetServiceByIdDetailedAsync(int id)
    {
        var serviceEntity = await _serviceRepository.GetOneAsync(x => x.Id == id, q => q.Include(s => s.Unit).Include(s => s.Projects));

        if (serviceEntity == null)
            return ServiceResult.NotFound($"Service with id {id} does not exist.");

        var service = ServiceFactory.ToModelDetailed(serviceEntity);
        return ServiceResult<ServiceModelDetailed>.Ok(service);
    }


    // UPDATE
    public async Task<IServiceResult> UpdateServiceAsync(ServiceUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var existingEntity = await _serviceRepository.GetOneAsync(x => x.Id == form.Id);
        if (existingEntity == null)
            return ServiceResult.NotFound("Service not found.");

        existingEntity.Name = form.Name;
        existingEntity.Price = form.Price;
        existingEntity.UnitId = form.UnitId;

        _serviceRepository.Update(existingEntity);
        var result = await _serviceRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed updating service.");

        var updatedEntity = await _serviceRepository.GetOneAsync(x => x.Id == form.Id, q => q.Include(s => s.Unit));
        return ServiceResult<ServiceModel>.Ok(ServiceFactory.ToModel(updatedEntity!));
    }


    // DELETE
    public async Task<IServiceResult> DeleteServiceAsync(ServiceModel model)
    {
        if (model == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var serviceEntity = await _serviceRepository.GetOneAsync(x => x.Id == model.Id);
        if (serviceEntity == null)
            return ServiceResult.NotFound("Service not found.");

        _serviceRepository.Delete(serviceEntity);
        var result = await _serviceRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting service failed.");

        return ServiceResult.Ok();
    }
}
