using PlantManagement.Commons.DBModels;

namespace PlantManagement.Repository.v1.Floor;

public partial class FloorRepository
{
    public async Task<bool> AddFloorAsync(FloorTb model)
    {
        try
        {
            await _context.FloorTbs.AddAsync(model).ConfigureAwait(false);

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
}