using Dapper;
using PlantManagement.Dto.v1.Lamp;

namespace PlantManagement.Repository.v1.Lamp;

public partial class LampRepository
{
    /// <summary>
    /// 할당안된 램프들 조회
    /// </summary>
    public async Task<List<GetLampDto>?> GetLampsAsync()
    {
        try
        {
            const string query = @"
                SELECT 
                    lt.LampSeq AS lampSeq, 
                    lt.LampName AS lampName
                     FROM LampTb AS lt
                    WHERE lt.LampSeq NOT IN(
	                    SELECT 
	                    fl.LampSeq
	                    FROM FloorLampTb AS fl
                    );
            ";
            
            var rows = await _dapper.QueryAsync<GetLampDto>(query).ConfigureAwait(false);

            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
    
    /// <summary>
    /// 해당층 램프 배정 저장 (기존 삭제 후 재삽입, 위치 포함)
    /// </summary>
    public async Task<bool> UpdateFloorLampsAsync(int floorSeq, List<(int lampSeq, double x, double y)> entries)
    {
        try
        {
            const string deleteQuery = "DELETE FROM FloorLampTb WHERE FloorSeq = @floorSeq";
            await _dapper.ExecuteAsync(deleteQuery, new { floorSeq }).ConfigureAwait(false);

            if (entries.Count > 0)
            {
                const string insertQuery = "INSERT INTO FloorLampTb (FloorSeq, LampSeq, PositionX, PositionY) VALUES (@floorSeq, @lampSeq, @posX, @posY)";
                foreach (var (lampSeq, x, y) in entries)
                {
                    await _dapper.ExecuteAsync(insertQuery, new { floorSeq, lampSeq, posX = x, posY = y }).ConfigureAwait(false);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    /// <summary>
    /// 해당층 램프 조회 (위치 포함)
    /// </summary>
    public async Task<List<GetFloorLampDto>?> GetFloorLampAsync(int floorSeq)
    {
        try
        {
            const string query = @"
                SELECT
                fl.LampSeq AS lampSeq,
                lt.LampName AS lampName,
                fl.PositionX AS positionX,
                fl.PositionY AS positionY
                FROM FloorLampTb AS fl
                JOIN LampTb AS lt ON lt.LampSeq = fl.LampSeq
                WHERE fl.FloorSeq = @floorSeq
            ";
            var rows = await _dapper.QueryAsync<GetFloorLampDto>(query, new { floorSeq }).ConfigureAwait(false);

            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
    
}
