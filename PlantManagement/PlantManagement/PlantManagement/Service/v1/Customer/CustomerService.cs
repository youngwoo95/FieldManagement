using PlantManagement.Comm.Logger;
using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Customer;
using PlantManagement.Repository.v1.Customer;

namespace PlantManagement.Service.v1.Customer;

public class CustomerService : ICustomerService
{
    private readonly ILogService _logService;
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(
        ILogService logService,
        ICustomerRepository customerRepository)
    {
        this._logService = logService;
        this._customerRepository = customerRepository;
    }
    
    /// <summary>
    /// 고객사 리스트 조회
    /// </summary>
    public async Task<List<GetCustomerDto>?> GetCustomerService()
    {
        try
        {
            var model = await _customerRepository.GetCustomerList().ConfigureAwait(false);
            return model;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 고객사 등록
    /// </summary>
    public async Task<bool> AddCustomerService(AddCustomerDto dto)
    {
        try
        {
            var model = new CustomerTb
            {
                Name = dto.customerName,
                Manager = dto.managerName,
                Gubun = dto.gubun,
                Department = dto.department,
                Tel = dto.tel,
                Email = dto.email,
                Address = dto.address,
                Memo = dto.memo
            };
            
            var addResult = await _customerRepository.AddCustomerAsync(model).ConfigureAwait(false);
            
            return addResult;
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }

    public async Task<bool> EditCustomerService(EditCustomerDto dto)
    {
        try
        {
            var model = new CustomerTb
            {
                CustomerSeq = dto.customerSeq,
                Name = dto.customerName,
                Manager = dto.managerName,
                Gubun = dto.gubun,
                Department = dto.department,
                Tel = dto.tel,
                Email = dto.email,
                Address = dto.address,
                Memo = dto.memo
            };

            return await _customerRepository.EditCustomerAsync(model).ConfigureAwait(false);
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
    public async Task<bool> RemoveCustomerService(List<int> customerSeq)
    {
        try
        {
            return await _customerRepository.RemoveCustomerAsync(customerSeq).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logService.LogMessage(ex.ToString());
            return false;
        }
    }
    
}
