using Dapper;
using PlantManagement.Dto.v1.Works;

namespace PlantManagement.Repository.v1.Works;

public partial class WorkRepository
{
    public async Task<List<GetWorkOrderInfoDto>?> GetWorkOrderInfosAsync()
    {
        try
        {
            const string query = @"
                SELECT 
                    ot.orderSeq AS orderSeq,
                    ot.customerSeq AS customerSeq,
                    c.`name` AS customerName,
                    ot.attach AS `attach`
                FROM OrderTb AS ot
                JOIN CustomerTb AS c ON c.customerSeq = ot.customerSeq
                WHERE ot.delYn = FALSE
            ";

            var rows = await _dapper.QueryAsync<GetWorkOrderInfoDto>(query);
            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    public async Task<List<GetWorkFacilityDto>?> GetWorkFacilitiesAsync()
    {
        try
        {
            const string query = @"
                SELECT 
                    ft.facilitySeq AS facilitySeq,
                    ft.facilityName AS facilityName
                FROM FacilityTb AS ft
                WHERE ft.delYn = FALSE
            ";

            var rows = await _dapper.QueryAsync<GetWorkFacilityDto>(query);
            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
    
    /// <summary>
    /// 작업 리스트
    /// </summary>
    public async Task<List<GetWorksListDto>?> GetWorksListAsync()
    {
        try
        {
            const string query = @"
                SELECT 
                    wt.workSeq AS workSeq,
                    ft.facilityName AS facilityName,
                    ct.`name` AS `customerName`,
                    CASE 
                     WHEN wt.`status` = 0 THEN '미착수'
                     WHEN wt.`status` = 1 THEN '진행중'
                     WHEN wt.`status` = 2 THEN '완료'
                     ELSE 'Unknown'
                     END AS 'statusName',
                    wt.startWorkDt AS startDt,
                    ot.attach AS attach
                    FROM WorkTb AS wt
                    JOIN FacilityTb AS ft ON ft.facilitySeq = wt.facilitySeq
                    JOIN OrderTb AS ot ON ot.orderSeq = wt.orderSeq
                    JOIN CustomerTb AS ct ON ct.customerSeq = ot.customerSeq
                    WHERE wt.delYn = FALSE
                        AND ft.delYn = FALSE 
                        AND ot.delYn = FALSE 
                        AND ct.delYn = FALSE
            ";
            
            var rows = await _dapper.QueryAsync<GetWorksListDto>(query);
            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
    
}
