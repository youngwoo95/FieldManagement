using Microsoft.EntityFrameworkCore;
using PlantManagement.Commons.DBModels;

namespace PlantManagement.Repository.v1.Orders;

public partial class OrderRepository
{
    /// <summary>
    /// 수주 등록
    /// </summary>
    public async Task<bool> AddOrderAsync(OrderTb model)
    {
        try
        {
            await _context.OrderTbs.AddAsync(model).ConfigureAwait(false);

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> EditOrderAsync(OrderTb model)
    {
        try
        {
            var target = await _context.OrderTbs.FindAsync(model.OrderSeq).ConfigureAwait(false);
            if (target is null || target.DelYn)
            {
                return false;
            }

            target.CustomerSeq = model.CustomerSeq;
            target.OrderQty = model.OrderQty;
            target.StartDt = model.StartDt;
            target.EndDt = model.EndDt;
            target.Attach = model.Attach;

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> RemoveOrderAsync(List<int> orderSeq)
    {
        try
        {
            if (orderSeq == null || orderSeq.Count == 0)
            {
                return false;
            }

            var targets = await _context.OrderTbs
                .Where(x => orderSeq.Contains(x.OrderSeq) && !x.DelYn)
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
