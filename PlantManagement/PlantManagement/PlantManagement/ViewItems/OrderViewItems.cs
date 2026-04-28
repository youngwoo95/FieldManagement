namespace PlantManagement.ViewItems;

/// <summary>
/// 수주 
/// </summary>
public class OrderViewItems
{
    public int orderSeq { get; set; }
    public int CustomerSeq { get; set; }
    public string Customer { get; set; } = string.Empty;
    public int OrderQty { get; set; }
    public string StartDt { get; set; } = string.Empty;
    public string EndDt { get; set; } = string.Empty;
    public string PdfFileName { get; set; } = string.Empty;
}
