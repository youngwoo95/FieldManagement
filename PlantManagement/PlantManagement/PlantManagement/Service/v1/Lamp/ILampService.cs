using PlantManagement.Dto.v1.Lamp;

namespace PlantManagement.Service.v1.Lamp;

public interface ILampService
{
    public Task<List<GetLampDto>?> GetLampService();

    public Task<List<GetFloorLampDto>?> GetFloorLampService(int floorSeq);

    public Task<bool> UpdateFloorLampService(int floorSeq, List<(int lampSeq, double x, double y)> entries);
}