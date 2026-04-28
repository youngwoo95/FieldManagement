using PlantManagement.Dto.v1.Orders;

namespace PlantManagement.Service.v1.Orders;

public interface IOrderService
{
    public Task<List<GetCustomerDto>?> GetCustomerService();

    public Task<bool> AddOrderService(AddOrderDto dto);

    public Task<bool> EditOrderService(EditOrderDto dto);

    public Task<List<GetOrdersDto>?> GetOrderService();
}
