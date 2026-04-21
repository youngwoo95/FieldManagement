namespace FieldManagement.Models;

/// <summary>
/// 수주
/// </summary>
public class OrderModel
{
    public string Customer { get; set; }
    public int OrderQty { get; set; }
    public string StartDt { get; set; }
    public string EndDt { get; set; }
}