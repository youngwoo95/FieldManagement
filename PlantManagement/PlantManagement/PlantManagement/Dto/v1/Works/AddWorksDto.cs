namespace PlantManagement.Dto.v1.Works;

public class AddWorksDto
{
    public string workSeq { get; set; } = string.Empty;
    public int orderSeq { get; set; }
    public int facilitySeq { get; set; }
    public int currentQty { get; set; }
    public DateTime startWorkDt { get; set; }
    public DateTime endWorkDt { get; set; }
    public int status { get; set; } = 0;
}
