namespace FieldManagement.Models;

public class CustomerModel
{
    public bool IsChecked { get; set; }
    
    /// <summary>
    /// 고객사명
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 담당자 명 
    /// </summary>
    public string Manager { get; set; } = string.Empty;
    
    /// <summary>
    /// 고객구분
    /// </summary>
    public string Gubun { get; set; } = string.Empty;
    
    /// <summary>
    /// 전화번호
    /// </summary>
    public string Tel { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
}
