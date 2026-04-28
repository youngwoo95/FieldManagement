namespace PlantManagement.Dto.v1.Notice;

public class GetNoticeDto
{
    /// <summary>
    /// 공지사항 PK
    /// </summary>
    public int noticeSeq { get; set; }
    
    /// <summary>
    /// 제목
    /// </summary>
    public string title { get; set; }
    
    /// <summary>
    /// 내용
    /// </summary>
    public string description { get; set; }
    
    /// <summary>
    /// 작성자
    /// </summary>
    public string createUser { get; set; }
    
    /// <summary>
    /// 작성일
    /// </summary>
    public DateTime createDt { get; set; }
}