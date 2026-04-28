using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Facility;

namespace PlantManagement.Repository.v1.Facility;

public interface IFacilityRepository
{
    Task<List<GetFacilityDto>?> GetFacilityList();
    Task<bool> AddFacilityAsync(FacilityTb model);
    Task<bool> EditFacilityAsync(FacilityTb model);
    Task<bool> RemoveFacilityAsync(List<int> facilitySeq);
}
