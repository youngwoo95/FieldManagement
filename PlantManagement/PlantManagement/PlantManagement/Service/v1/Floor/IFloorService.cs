using PlantManagement.Dto.v1.Floor;

namespace PlantManagement.Service.v1.Floor;

public interface IFloorService
{
    Task<List<GetFloorDto>?> GetFloorService();

    Task<bool> AddFloorService(AddFloorDto dto);
}
