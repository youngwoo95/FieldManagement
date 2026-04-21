using FieldManagement.Commons.DBModels;

namespace FieldManagement.Repository.Customer;

public interface ICustomerRepository
{
    /// <summary>
    /// 고객사 추가
    /// </summary>
    public Task<bool> AddCustomerAsync(CustomerTb model);
    
    // ============= Dapper
    
}