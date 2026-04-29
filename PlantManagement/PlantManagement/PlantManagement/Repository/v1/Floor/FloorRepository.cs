using System.Data;
using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Commons.Repository;

namespace PlantManagement.Repository.v1.Floor;

public partial class FloorRepository : IFloorRepository
{
    private readonly PlantContext _context; // EFCore
    private readonly IDbConnection _dapper; // Dapper
    private readonly ILogService _logService;

    public FloorRepository(
        PlantContext context,
        IDbConnection dapper,
        ILogService logService)
    {
        _context = context;
        _dapper = dapper;
        _logService = logService;
    }

  
}
