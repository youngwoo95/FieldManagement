using Dapper;
using PlantManagement.Dto.v1.Floor;

namespace PlantManagement.Repository.v1.Floor;

public partial class FloorRepository
{
    public async Task<List<GetFloorDto>?> GetFloorListAsync()
    {
        try
        {
            const string query = @"
                SELECT 
                    ft.floorSeq AS floorSeq,
                    ft.`name` AS `name`
                FROM FloorTb AS ft
            ";

            var rows = await _dapper.QueryAsync<GetFloorDto>(query);
            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
}
