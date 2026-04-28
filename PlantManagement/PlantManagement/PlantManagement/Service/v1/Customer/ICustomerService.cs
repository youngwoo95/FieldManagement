using PlantManagement.Dto.v1.Customer;

namespace PlantManagement.Service.v1.Customer;

public interface ICustomerService
{
    /// <summary>
    /// 고객사 조회
    /// </summary>
    Task<List<GetCustomerDto>?> GetCustomerService();
    
    /// <summary>
    /// 고객사 추가
    /// </summary>
    Task<bool> AddCustomerService(AddCustomerDto dto);
    
    /// <summary>
    /// 고객사 수정
    /// </summary>
    Task<bool> EditCustomerService(EditCustomerDto dto);

    /// <summary>
    /// 고객사 삭제
    /// </summary>
    Task<bool> RemoveCustomerService(List<int> customerSeq);
}
