namespace PlantManagement.Dto.v1.Facility;

public class AddFacilityDto
{
    /// <summary>
    /// 설비명
    /// </summary>
    public string facilityName { get; set; }
    
    /// <summary>
    /// 제조사
    /// </summary>
    public string maker { get; set; }
    
    /// <summary>
    /// 목적
    /// </summary>
    public string purpose { get; set; }
}