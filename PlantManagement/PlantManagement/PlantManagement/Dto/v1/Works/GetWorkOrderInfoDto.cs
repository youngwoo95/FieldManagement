namespace PlantManagement.Dto.v1.Works;

public class GetWorkOrderInfoDto
{
    public int orderSeq { get; set; }
    public int customerSeq { get; set; }
    public string customerName { get; set; } = string.Empty;
    public string attach { get; set; } = string.Empty;
}
