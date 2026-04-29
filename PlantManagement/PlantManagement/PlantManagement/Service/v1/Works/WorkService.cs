using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Works;
using PlantManagement.Repository.v1.Works;

namespace PlantManagement.Service.v1.Works;

public class WorkService : IWorkService
{
    private readonly ILogService _logService;
    private readonly IWorkRepository _workRepository;

    public WorkService(
        ILogService logService,
        IWorkRepository workRepository)
    {
        _logService = logService;
        _workRepository = workRepository;
    }

    public async Task<List<GetWorkOrderInfoDto>?> GetWorkOrderInfosService()
    {
        try
        {
            return await _workRepository.GetWorkOrderInfosAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    public async Task<List<GetWorkFacilityDto>?> GetWorkFacilitiesService()
    {
        try
        {
            return await _workRepository.GetWorkFacilitiesAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    public async Task<bool> AddWorksService(AddWorksDto dto)
    {
        try
        {
            var model = new WorkTb
            {
                WorkSeq = dto.workSeq,
                OrderSeq = dto.orderSeq,
                FacilitySeq = dto.facilitySeq,
                CurrentQty = dto.currentQty,
                StartWorkDt = dto.startWorkDt,
                EndWorkDt = dto.endWorkDt == default ? null : dto.endWorkDt,
                Status = dto.status,
                DelYn = false
            };

            return await _workRepository.AddWorksAsync(model).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<List<GetWorksListDto>?> GetWorksListService()
    {
        try
        {
            return await _workRepository.GetWorksListAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
}
