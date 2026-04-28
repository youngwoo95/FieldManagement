namespace PlantManagement.Dto.v1.Customer;

public class GetCustomerDto
{
    public int customerSeq { get; set; }
    public string customerName { get; set; } = string.Empty;
    public string managerName { get; set; } = string.Empty;
    public string gubun { get; set; } = string.Empty;
    public string? tel { get; set; }
    public string? address { get; set; }
    public string? department { get; set; }
    public string? email { get; set; }
    public string? memo { get; set; }
}
