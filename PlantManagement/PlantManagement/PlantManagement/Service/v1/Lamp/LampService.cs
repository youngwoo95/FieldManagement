using PlantManagement.Comm.Logger;
using PlantManagement.Dto.v1.Lamp;
using PlantManagement.Repository.v1.Lamp;

namespace PlantManagement.Service.v1.Lamp;

public class LampService : ILampService
{
    private readonly ILampRepository _lampRepository;
    private readonly ILogService _logService;

    public LampService(
        ILampRepository lampRepository,
        ILogService logService)
    {
        this._lampRepository = lampRepository;
        this._logService = logService;
    }
    
    /// <summary>
    /// 할당안된 램프들 조회
    /// </summary>
    public async Task<List<GetLampDto>?> GetLampService()
    {
        try
        {
            return await _lampRepository.GetLampsAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 층에 해당하는 램프 조회 (위치 포함)
    /// </summary>
    public async Task<List<GetFloorLampDto>?> GetFloorLampService(int floorSeq)
    {
        try
        {
            return await _lampRepository.GetFloorLampAsync(floorSeq).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 해당층 램프 배정 저장 (위치 포함)
    /// </summary>
    public async Task<bool> UpdateFloorLampService(int floorSeq, List<(int lampSeq, double x, double y)> entries)
    {
        try
        {
            return await _lampRepository.UpdateFloorLampsAsync(floorSeq, entries).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
}
