using PlantManagement.Commons.DBModels;
using PlantManagement.Dto.v1.Customer;

namespace PlantManagement.Repository.v1.Customer;

public interface ICustomerRepository
{
    /// <summary>
    /// 고객사 등록
    /// </summary>
    Task<bool> AddCustomerAsync(CustomerTb model);
    
    /// <summary>
    /// 고객사 수정
    /// </summary>
    Task<bool> EditCustomerAsync(CustomerTb model);
    
    /// <summary>
    /// 고객사 리스트 조회
    /// </summary>
    /// <returns></returns>
    Task<List<GetCustomerDto>?> GetCustomerList();

    /// <summary>
    /// 고객사 삭제
    /// </summary>
    Task<bool> RemoveCustomerAsync(List<int> customerSeq);
}
