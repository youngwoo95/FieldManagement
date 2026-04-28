using System.Data;
using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Commons.Repository;
using PlantManagement.Dto.v1.Customer;

namespace PlantManagement.Repository.v1.Customer;

public partial class CustomerRepository : ICustomerRepository
{
    private readonly PlantContext _context; // EFCore
    private readonly IDbConnection _dapper; // Dapper
    private readonly ILogService _logService;

    public CustomerRepository(
        PlantContext context,
        IDbConnection dapper,
        ILogService logService)
    {
        this._context = context;
        this._dapper = dapper;
        this._logService = logService;
    }


 
}