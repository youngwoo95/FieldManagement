namespace PlantManagement.Dto.v1.Works;

public class GetWorksListDto
{
    public string workSeq { get; set; }
    public string facilityName { get; set; }
    public string customerName { get; set; }
    public string statusName { get; set; }
    public DateTime startDt { get; set; }
    public string attach { get; set; }
}