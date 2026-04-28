using System.Data;
using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Commons.Repository;
using PlantManagement.Dto.v1.Orders;

namespace PlantManagement.Repository.v1.Orders;

public partial class OrderRepository : IOrderRepository
{
    private readonly PlantContext _context; // EFCore
    private readonly IDbConnection _dapper; // Dapper
    private readonly ILogService _logService;

    public OrderRepository(
        PlantContext context,
        IDbConnection dapper,
        ILogService logService)
    {
        this._context = context;
        this._dapper = dapper;
        this._logService = logService;
    }

   
}