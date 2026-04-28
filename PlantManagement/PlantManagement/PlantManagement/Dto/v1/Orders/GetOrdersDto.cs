namespace PlantManagement.Dto.v1.Orders;

public class GetOrdersDto
{
    public int orderSeq { get; set; }
    public int customerSeq { get; set; }
    public string name { get; set; }
    public int orderQty { get; set; }
    
    public string startDt { get; set; }
    public string endDt { get; set; }
    public string attach { get; set; }
}
