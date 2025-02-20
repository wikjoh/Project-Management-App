﻿using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;

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

        var serviceEntity = ServiceFactory.Create(form);

        await _serviceRepository.CreateAsync(serviceEntity);
        var result = await _serviceRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating service.");

        var service = ServiceFactory.Create(serviceEntity);
        return ServiceResult<ServiceModel>.Ok(service);
    }


    // READ
    public async Task<IServiceResult> GetAllServicesAsync()
    {
        var serviceEntities = await _serviceRepository.GetAllAsync();
        var serviceList = serviceEntities != null ? serviceEntities.Select(x => ServiceFactory.Create(x)) : [];

        return ServiceResult<IEnumerable<ServiceModel>>.Ok(serviceList);
    }


    public async Task<IServiceResult> GetServiceByIdAsync(int id)
    {
        var serviceEntity = await _serviceRepository.GetOneAsync(x => x.Id == id);

        if (serviceEntity == null)
            return ServiceResult.NotFound($"Service with id {id} does not exist.");

        var service = ServiceFactory.Create(serviceEntity);
        return ServiceResult<ServiceModel>.Ok(service);
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

        var updatedEntity = await _serviceRepository.GetOneAsync(x => x.Id == form.Id);
        return ServiceResult<ServiceModel>.Ok(ServiceFactory.Create(updatedEntity!));
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
