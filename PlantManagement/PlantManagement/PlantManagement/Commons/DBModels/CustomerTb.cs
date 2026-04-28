using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

/// <summary>
/// 고객사 테이블
/// </summary>
public partial class CustomerTb
{
    /// <summary>
    /// 고객사PK
    /// </summary>
    public int CustomerSeq { get; set; }

    /// <summary>
    /// 고객사 명
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 담당자명
    /// </summary>
    public string? Manager { get; set; }

    /// <summary>
    /// 고객구분
    /// </summary>
    public string? Gubun { get; set; }

    /// <summary>
    /// 부서명
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// 연락처
    /// </summary>
    public string? Tel { get; set; }

    /// <summary>
    /// 이메일
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 주소
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// 비고
    /// </summary>
    public string? Memo { get; set; }

    public DateTime CreateDt { get; set; }

    /// <summary>
    /// 삭제여부
    /// </summary>
    public bool DelYn { get; set; }

    public virtual ICollection<OrderTb> OrderTbs { get; set; } = new List<OrderTb>();
}
