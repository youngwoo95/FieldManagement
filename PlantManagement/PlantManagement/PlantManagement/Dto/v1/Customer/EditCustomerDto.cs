namespace PlantManagement.Dto.v1.Customer;

public class EditCustomerDto
{
    /// <summary>
    /// 고객사 PK
    /// </summary>
    public int customerSeq { get; set; }
    
    /// 고객사 명
    /// </summary>
    public string customerName { get; set; }
    
    /// <summary>
    /// 매니저 명
    /// </summary>
    public string managerName { get; set; }
    
    /// <summary>
    /// 구분
    /// </summary>
    public string gubun { get; set; }
    
    /// <summary>
    /// 부서명
    /// </summary>
    public string department { get; set; }
    
    /// <summary>
    /// 전화번호
    /// </summary>
    public string tel { get; set; }

    /// <summary>
    /// 이메일
    /// </summary>
    public string email { get; set; }
    
    /// <summary>
    /// 주소 
    /// </summary>
    public string address { get; set; }
    
    /// <summary>
    /// 메모
    /// </summary>
    public string memo { get; set; }
}