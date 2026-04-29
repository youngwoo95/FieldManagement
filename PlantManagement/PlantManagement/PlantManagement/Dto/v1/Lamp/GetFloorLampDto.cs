namespace PlantManagement.Dto.v1.Lamp;

public class GetFloorLampDto
{
    public int lampSeq { get; set; }
    public string lampName { get; set; } = string.Empty;
    public double positionX { get; set; }
    public double positionY { get; set; }
}
