using System.Data;
using System.Windows.Controls;
using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Commons.Repository;
using PlantManagement.Dto.v1.Facility;

namespace PlantManagement.Repository.v1.Facility;

public partial class FacilityRepository : IFacilityRepository
{
    private readonly PlantContext _context; // EFCore
    private readonly IDbConnection _dapper; // Dapper
    private readonly ILogService _logService;

    public FacilityRepository(
        PlantContext context,
        IDbConnection dapper,
        ILogService logService)
    {
        this._context = context;
        this._dapper = dapper;
        this._logService = logService;
    }


    
}