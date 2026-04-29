using System.Data;
using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Commons.Repository;
using PlantManagement.Dto.v1.Works;

namespace PlantManagement.Repository.v1.Works;

public partial class WorkRepository : IWorkRepository
{
    private readonly PlantContext _context; // EFCore
    private readonly IDbConnection _dapper; // Dapper
    private readonly ILogService _logService;

    public WorkRepository(
        PlantContext context,
        IDbConnection dapper,
        ILogService logService)
    {
        _context = context;
        _dapper = dapper;
        _logService = logService;
    }


   
}
