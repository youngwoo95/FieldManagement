using Microsoft.EntityFrameworkCore;
using PlantManagement.Commons.DBModels;

namespace PlantManagement.Repository.v1.Facility;

public partial class FacilityRepository
{
    public async Task<bool> AddFacilityAsync(FacilityTb model)
    {
        try
        {
            await _context.FacilityTbs.AddAsync(model).ConfigureAwait(false);
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> EditFacilityAsync(FacilityTb model)
    {
        try
        {
            var target = await _context.FacilityTbs.FindAsync(model.FacilitySeq).ConfigureAwait(false);
            if (target is null)
            {
                return false;
            }

            target.FacilityName = model.FacilityName;
            target.Maker = model.Maker;
            target.Purpose = model.Purpose;

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> RemoveFacilityAsync(List<int> facilitySeq)
    {
        try
        {
            if (facilitySeq == null || facilitySeq.Count == 0)
            {
                return false;
            }

            var targets = await _context.FacilityTbs
                .Where(x => facilitySeq.Contains(x.FacilitySeq) && !x.DelYn)
                .ToListAsync()
                .ConfigureAwait(false);

            if (targets.Count == 0)
            {
                return false;
            }

            foreach (var target in targets)
            {
                target.DelYn = true;
            }

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
}
