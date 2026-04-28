using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

/// <summary>
/// 설비이력
/// </summary>
public partial class FacilityLogTb
{
    /// <summary>
    /// 로그 PK
    /// </summary>
    public int LogSeq { get; set; }

    /// <summary>
    /// 설비 FK
    /// </summary>
    public int FacilitySeq { get; set; }

    /// <summary>
    /// 작업지시번호 FK
    /// </summary>
    public string WorkSeq { get; set; } = null!;

    /// <summary>
    /// 메모
    /// </summary>
    public string? Memo { get; set; }

    public DateTime CreateDt { get; set; }

    public virtual FacilityTb FacilitySeqNavigation { get; set; } = null!;

    public virtual WorkTb WorkSeqNavigation { get; set; } = null!;
}
