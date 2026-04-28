using PlantManagement.Dto.v1.Facility;

namespace PlantManagement.Service.v1.Facility;

public interface IFacilityService
{
    Task<List<GetFacilityDto>?> GetFacilityService();
    Task<bool> AddFacilityService(AddFacilityDto dto);
    Task<bool> EditFacilityService(EditFacilityDto dto);
    
    Task<bool> RemoveFacilityService(List<int> facilitySeq);
}
