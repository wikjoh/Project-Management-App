using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Models.ServiceResult;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class ServiceUnitService(IServiceUnitRepository serviceUnitRepository) : IServiceUnitService
{
    private readonly IServiceUnitRepository _serviceUnitRepository = serviceUnitRepository;


    // CREATE
    public async Task<IServiceResult> CreateServiceUnitAsync(ServiceUnitRegistrationForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Invalid registration form.");

        bool? serviceUnitExists = await _serviceUnitRepository.ExistsAsync(x => x.Unit == form.Unit);
        if (serviceUnitExists == true)
            return ServiceResult.AlreadyExists($"Unit {form.Unit} already exists.");

        var serviceUnitEntity = ServiceUnitFactory.ToEntity(form);

        await _serviceUnitRepository.CreateAsync(serviceUnitEntity);
        var result = await _serviceUnitRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed creating service unit.");

        var serviceUnit = ServiceUnitFactory.ToModel(serviceUnitEntity);
        return ServiceResult<ServiceUnitModel>.Ok(serviceUnit);
    }


    // READ
    public async Task<IServiceResult> GetAllServiceUnitsAsync()
    {
        var serviceUnits = await _serviceUnitRepository.GetAllAsync();
        var serviceUnitList = serviceUnits != null ? serviceUnits.Select(x => ServiceUnitFactory.ToModel(x)) : [];

        return ServiceResult<IEnumerable<ServiceUnitModel>>.Ok(serviceUnitList);
    }

    public async Task<IServiceResult> GetAllServiceUnitsDetailedAsync()
    {
        var serviceUnits = await _serviceUnitRepository.GetAllAsync(q => q.Include(su => su.Services));
        var serviceUnitList = serviceUnits != null ? serviceUnits.Select(x => ServiceUnitFactory.ToModelDetailed(x)) : [];

        return ServiceResult<IEnumerable<ServiceUnitModelDetailed>>.Ok(serviceUnitList);
    }

    public async Task<IServiceResult> GetServiceUnitByIdAsync(int id)
    {
        var serviceUnitEntity = await _serviceUnitRepository.GetOneAsync(x => x.Id == id);

        if (serviceUnitEntity == null)
            return ServiceResult.NotFound($"Service unit with id {id} does not exist.");

        var serviceUnit = ServiceUnitFactory.ToModel(serviceUnitEntity);
        return ServiceResult<ServiceUnitModel>.Ok(serviceUnit);
    }


    // UPDATE
    public async Task<IServiceResult> UpdateServiceUnitAsync(ServiceUnitUpdateForm form)
    {
        if (form == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var existingEntity = await _serviceUnitRepository.GetOneAsync(x => x.Id == form.Id);
        if (existingEntity == null)
            return ServiceResult.NotFound($"Service unit not found.");

        existingEntity.Unit = form.Unit;

        _serviceUnitRepository.Update(existingEntity);
        var result = await _serviceUnitRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Failed updating service unit.");

        var updatedEntity = await _serviceUnitRepository.GetOneAsync(x => x.Id == form.Id);
        return ServiceResult<ServiceUnitModel>.Ok(ServiceUnitFactory.ToModel(updatedEntity!));
    }


    // DELETE
    public async Task<IServiceResult> DeleteServiceUnitAsync(ServiceUnitModel model)
    {
        if (model == null)
            return ServiceResult.BadRequest("Form cannot be empty.");

        var serviceUnitEntity = await _serviceUnitRepository.GetOneAsync(x => x.Id == model.Id);
        if (serviceUnitEntity == null)
            return ServiceResult.NotFound("Service unit not found");

        _serviceUnitRepository.Delete(serviceUnitEntity);
        var result = await _serviceUnitRepository.SaveAsync() > 0;
        if (!result)
            return ServiceResult.InternalServerError("Deleting sevice unit failed.");

        return ServiceResult.Ok();
    }
}
