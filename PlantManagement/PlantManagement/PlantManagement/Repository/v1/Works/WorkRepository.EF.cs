using PlantManagement.Commons.DBModels;

namespace PlantManagement.Repository.v1.Works;

public partial class WorkRepository
{
    /// <summary>
    /// 작업 추가
    /// </summary>
    public async Task<bool> AddWorksAsync(WorkTb model)
    {
        try
        {
            await _context.WorkTbs.AddAsync(model).ConfigureAwait(false);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
}
