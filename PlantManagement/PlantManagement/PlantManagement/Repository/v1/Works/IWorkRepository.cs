using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Works;

namespace PlantManagement.Repository.v1.Works;

public interface IWorkRepository
{
    Task<List<GetWorkOrderInfoDto>?> GetWorkOrderInfosAsync();
    Task<List<GetWorkFacilityDto>?> GetWorkFacilitiesAsync();

    /// <summary>
    /// 작업 추가
    /// </summary>
    Task<bool> AddWorksAsync(WorkTb model);

    /// <summary>
    /// 작업 리스트
    /// </summary>
    Task<List<GetWorksListDto>?> GetWorksListAsync();
}
