using System.Data;
using PlantManagement.Comm.Logger;
using PlantManagement.Commons.Repository;
using PlantManagement.Dto.v1.Lamp;

namespace PlantManagement.Repository.v1.Lamp;

public partial class LampRepository : ILampRepository
{
    private readonly PlantContext _context; // EFCore
    private readonly IDbConnection _dapper; // Dapper
    private readonly ILogService _logService;

    public LampRepository(
        PlantContext context,
        IDbConnection dapper,
        ILogService logService)
    {
        this._context = context;
        this._dapper = dapper;
        this._logService = logService;
    }

  
}