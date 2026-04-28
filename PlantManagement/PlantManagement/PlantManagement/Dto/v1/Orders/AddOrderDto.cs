namespace PlantManagement.Dto.v1.Orders;

public class AddOrderDto
{
    /// <summary>
    /// 고객사 seq
    /// </summary>
    public int customerSeq { get; set; }
    
    /// <summary>
    /// 요청수량
    /// </summary>
    public int orderQty { get; set; }
    
    /// <summary>
    /// 시작일자
    /// </summary>
    public DateTime startDt { get; set; }
    
    /// <summary>
    /// 종료일자
    /// </summary>
    public DateTime endDt { get; set; }
    
    /// <summary>
    /// 도면
    /// </summary>
    public string attach { get; set; }
    
}