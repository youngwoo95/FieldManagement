using PlantManagement.Dto.v1.Works;

namespace PlantManagement.Service.v1.Works;

public interface IWorkService
{
    Task<List<GetWorkOrderInfoDto>?> GetWorkOrderInfosService();
    Task<List<GetWorkFacilityDto>?> GetWorkFacilitiesService();
    Task<bool> AddWorksService(AddWorksDto dto);

    Task<List<GetWorksListDto>?> GetWorksListService();
}
