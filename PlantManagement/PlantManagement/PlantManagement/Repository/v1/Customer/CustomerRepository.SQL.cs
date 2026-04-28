using Dapper;
using PlantManagement.Dto.v1.Customer;

namespace PlantManagement.Repository.v1.Customer;

public partial class CustomerRepository
{
    /// <summary>
    /// 고객사 목록 조회
    /// </summary>
    public async Task<List<GetCustomerDto>?> GetCustomerList()
    {
        try
        {
            const string query = @"
                SELECT 
                    c.customerSeq AS customerSeq,
                    c.`name` AS customerName,
                    c.manager AS managerName,
                    c.gubun AS gubun,
                    c.tel,
                    c.address,
                    c.department,
                    c.email,
                    c.memo
                    FROM CustomerTb AS c
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
}
