namespace PlantManagement.Dto.v1.Orders;

public class GetCustomerDto
{
    /// <summary>
    /// 고객사 PK
    /// </summary>
    public int customerSeq { get; set; }
    
    /// <summary>
    /// 고객사 명
    /// </summary>
    public string customerName { get; set; }
}