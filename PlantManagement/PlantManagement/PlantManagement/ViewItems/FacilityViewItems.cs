namespace PlantManagement.ViewItems;

public class FacilityViewItems
{
    public int Seq { get; set; }
    
    /// <summary>
    /// 체크여부
    /// </summary>
    public bool IsChecked { get; set; }
    
    /// <summary>
    /// 설비명
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 제조사
    /// </summary>
    public string Maker { get; set; }
    
    /// <summary>
    /// 용도
    /// </summary>
    public string Purpose { get; set; }
}