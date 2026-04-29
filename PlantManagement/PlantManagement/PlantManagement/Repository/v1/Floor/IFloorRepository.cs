using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Floor;

namespace PlantManagement.Repository.v1.Floor;

public interface IFloorRepository
{
    Task<List<GetFloorDto>?> GetFloorListAsync();

    /// <summary>
    /// 층추가 
    /// </summary>
    Task<bool> AddFloorAsync(FloorTb model);
}
