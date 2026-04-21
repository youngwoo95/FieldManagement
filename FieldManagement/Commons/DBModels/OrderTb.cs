using System;
using System.Collections.Generic;

namespace FieldManagement.Commons.DBModels;

/// <summary>
/// 수주테이블
/// </summary>
public partial class OrderTb
{
    /// <summary>
    /// 수주 PK
    /// </summary>
    public int OrderSeq { get; set; }

    /// <summary>
    /// 고객사 FK
    /// </summary>
    public int CustomerSeq { get; set; }

    /// <summary>
    /// 요청수량
    /// </summary>
    public int OrderQty { get; set; }

    /// <summary>
    /// 요청일
    /// </summary>
    public DateOnly StartDt { get; set; }

    /// <summary>
    /// 마감일
    /// </summary>
    public DateOnly EndDt { get; set; }

    /// <summary>
    /// 첨부파일
    /// </summary>
    public string? Attach { get; set; }

    /// <summary>
    /// 삭제여부
    /// </summary>
    public bool DelYn { get; set; }

    public virtual CustomerTb CustomerSeqNavigation { get; set; } = null!;

    public virtual ICollection<WorkTb> WorkTbs { get; set; } = new List<WorkTb>();
}
