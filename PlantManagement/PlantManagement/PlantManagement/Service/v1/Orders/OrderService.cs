using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Orders;
using PlantManagement.Repository.v1.Orders;

namespace PlantManagement.Service.v1.Orders;

public class OrderService : IOrderService
{
    private readonly ILogService _logService;
    private readonly IOrderRepository _orderRepository;

    public OrderService(
        ILogService logService,
        IOrderRepository orderRepository)
    {
        this._logService = logService;
        this._orderRepository = orderRepository;
    }
    
    /// <summary>
    /// 고객사 조회
    /// </summary>
    public async Task<List<GetCustomerDto>?> GetCustomerService()
    {
        try
        {
            return await _orderRepository.GetCustomerList().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    public async Task<bool> AddOrderService(AddOrderDto dto)
    {
        try
        {
            var model = new OrderTb
            {
                CustomerSeq = dto.customerSeq,
                OrderQty = dto.orderQty,
                StartDt = DateOnly.FromDateTime(dto.startDt),
                EndDt = DateOnly.FromDateTime(dto.endDt),
                Attach = dto.attach
            };
            
            var addResult = await _orderRepository.AddOrderAsync(model).ConfigureAwait(false);
            
            return addResult;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> EditOrderService(EditOrderDto dto)
    {
        try
        {
            var model = new OrderTb
            {
                OrderSeq = dto.orderSeq,
                CustomerSeq = dto.customerSeq,
                OrderQty = dto.orderQty,
                StartDt = DateOnly.FromDateTime(dto.startDt),
                EndDt = DateOnly.FromDateTime(dto.endDt),
                Attach = dto.attach
            };

            return await _orderRepository.EditOrderAsync(model).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    /// <summary>
    /// 수주 정보 조회
    /// </summary>
    public async Task<List<GetOrdersDto>?> GetOrderService()
    {
        try
        {
            return await _orderRepository.GetOrderList().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }
}
