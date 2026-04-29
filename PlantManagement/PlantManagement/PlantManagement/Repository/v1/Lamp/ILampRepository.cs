using PlantManagement.Dto.v1.Lamp;

namespace PlantManagement.Repository.v1.Lamp;

public interface ILampRepository
{
    /// <summary>
    /// 할당안된 램프들 조회
    /// </summary>
    public Task<List<GetLampDto>?> GetLampsAsync();

    /// <summary>
    /// 해당층 램프 조회 (위치 포함)
    /// </summary>
    public Task<List<GetFloorLampDto>?> GetFloorLampAsync(int floorSeq);

    /// <summary>
    /// 해당층 램프 배정 저장 (기존 삭제 후 재삽입, 위치 포함)
    /// </summary>
    public Task<bool> UpdateFloorLampsAsync(int floorSeq, List<(int lampSeq, double x, double y)> entries);
}