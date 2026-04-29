using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Orders;

namespace PlantManagement.Repository.v1.Orders;

public interface IOrderRepository
{
    /// <summary>
    /// 고객사 리스트 조회
    /// </summary>
    public Task<List<GetCustomerDto>?> GetCustomerList();

    /// <summary>
    /// 수주 등록
    /// </summary>
    public Task<bool> AddOrderAsync(OrderTb model);
    public Task<bool> EditOrderAsync(OrderTb model);
    public Task<bool> RemoveOrderAsync(List<int> orderSeq);
    
    /// <summary>
    /// 수주 리스트 조회 
    /// </summary>
    public Task<List<GetOrdersDto>?> GetOrderList();
}
