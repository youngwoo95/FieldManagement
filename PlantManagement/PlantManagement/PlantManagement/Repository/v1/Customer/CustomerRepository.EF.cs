using Microsoft.EntityFrameworkCore;
using PlantManagement.Commons.DBModels;

namespace PlantManagement.Repository.v1.Customer;

public partial class CustomerRepository
{
    /// <summary>
    /// 고객사 등록
    /// </summary>
    public async Task<bool> AddCustomerAsync(CustomerTb model)
    {
        try
        {
            await _context.CustomerTbs.AddAsync(model).ConfigureAwait(false);

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> EditCustomerAsync(CustomerTb model)
    {
        try
        {
            var target = await _context.CustomerTbs.FindAsync(model.CustomerSeq).ConfigureAwait(false);
            if (target is null)
            {
                return false;
            }

            target.Name = model.Name;
            target.Manager = model.Manager;
            target.Gubun = model.Gubun;
            target.Department = model.Department;
            target.Tel = model.Tel;
            target.Email = model.Email;
            target.Address = model.Address;
            target.Memo = model.Memo;

            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
    
    /// <summary>
    /// 고객사 삭제
    /// </summary>
    public async Task<bool> RemoveCustomerAsync(List<int> customerSeq)
    {
        try
        {
            if (customerSeq == null || customerSeq.Count == 0)
            {
                return false;
            }

            var targets = await _context.CustomerTbs
                .Where(x => customerSeq.Contains(x.CustomerSeq) && !x.DelYn)
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
