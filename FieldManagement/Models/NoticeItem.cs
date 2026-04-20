namespace FieldManagement.Models;

public class NoticeItem
{
    /// <summary>
    /// 순서 PK
    /// </summary>
    public int No { get; set; }
    
    /// <summary>
    /// 제목
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// 내용
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// 작성자
    /// </summary>
    public string CreateUser { get; set; } = string.Empty;
    
    /// <summary>
    /// 작성일
    /// </summary>
    public string CreateDt { get; set; } = string.Empty;
}