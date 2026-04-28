using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Facility;
using PlantManagement.Repository.v1.Facility;

namespace PlantManagement.Service.v1.Facility;

public class FacilityService : IFacilityService
{
    private readonly ILogService _logService;
    private readonly IFacilityRepository _facilityRepository;

    public FacilityService(
        ILogService logService,
        IFacilityRepository facilityRepository)
    {
        _logService = logService;
        _facilityRepository = facilityRepository;
    }

    public async Task<List<GetFacilityDto>?> GetFacilityService()
    {
        try
        {
            return await _facilityRepository.GetFacilityList().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    public async Task<bool> AddFacilityService(AddFacilityDto dto)
    {
        try
        {
            var model = new FacilityTb
            {
                FacilityName = dto.facilityName,
                Maker = dto.maker,
                Purpose = dto.purpose
            };

            return await _facilityRepository.AddFacilityAsync(model).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> EditFacilityService(EditFacilityDto dto)
    {
        try
        {
            var model = new FacilityTb
            {
                FacilitySeq = dto.facilitySeq,
                FacilityName = dto.facilityName,
                Maker = dto.maker,
                Purpose = dto.purpose
            };

            return await _facilityRepository.EditFacilityAsync(model).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> RemoveFacilityService(List<int> facilitySeq)
    {
        try
        {
            return await _facilityRepository.RemoveFacilityAsync(facilitySeq).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
}
