using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Floor;
using PlantManagement.Repository.v1.Floor;

namespace PlantManagement.Service.v1.Floor;

public class FloorService : IFloorService
{
    private readonly ILogService _logService;
    private readonly IFloorRepository _floorRepository;

    public FloorService(
        ILogService logService,
        IFloorRepository floorRepository)
    {
        _logService = logService;
        _floorRepository = floorRepository;
    }

    public async Task<List<GetFloorDto>?> GetFloorService()
    {
        try
        {
            return await _floorRepository.GetFloorListAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 층 추가
    /// </summary>
    public async Task<bool> AddFloorService(AddFloorDto dto)
    {
        try
        {
            var model = new FloorTb
            {
                Name = dto.floorName,
                Attach = dto.attach
            };
            
            var result = await _floorRepository.AddFloorAsync(model).ConfigureAwait(false);
            return result;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
}
