using Dapper;
using PlantManagement.Dto.v1.Orders;

namespace PlantManagement.Repository.v1.Orders;

public partial class OrderRepository
{
    /// <summary>
    /// 고객사 List 조회
    /// </summary>
    public async Task<List<GetCustomerDto>?> GetCustomerList()
    {
        try
        {
            const string query = @"
                SELECT 
                    c.customerSeq AS customerSeq,
                    c.`name` AS customerName
                    FROM CustomerTb AS c
                    where c.delYn = FALSE
            ";
            
            var rows = await _dapper.QueryAsync<GetCustomerDto>(query);

            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
    
    /// <summary>
    /// 수주 리스트 조회
    /// </summary>
    public async Task<List<GetOrdersDto>?> GetOrderList()
    {
        try
        {
            const string query = @"
               SELECT 
                o.orderSeq AS orderSeq,
                o.customerSeq AS customerSeq,
                c.`name`  AS `name`,
                o.orderQty AS orderQty,
                o.startDt AS startDt,
                o.endDt AS endDt,
                o.attach AS attach
                FROM OrderTb AS o
                JOIN CustomerTb AS c ON c.customerSeq = o.customerSeq
                WHERE o.delYn = FALSE
            ";

            var rows = await _dapper.QueryAsync<GetOrdersDto>(query);

            return rows.ToList();
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
    
}
