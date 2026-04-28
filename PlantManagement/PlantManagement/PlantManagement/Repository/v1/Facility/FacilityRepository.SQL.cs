using Dapper;
using PlantManagement.Dto.v1.Facility;

namespace PlantManagement.Repository.v1.Facility;

public partial class FacilityRepository
{
    /// <summary>
    /// 설비목록 조회
    /// </summary>
    public async Task<List<GetFacilityDto>?> GetFacilityList()
    {
        try
        {
            const string query = @"
                    SELECT 
                    ft.facilitySeq AS facilitySeq,
                    ft.facilityName AS facilityName,
                    ft.maker AS maker,
                    ft.purpose AS purpose
                    FROM FacilityTb AS ft
                    WHERE ft.delYn = FALSE;
            ";
            
            var rows = await _dapper.QueryAsync<GetFacilityDto>(query);

            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
}