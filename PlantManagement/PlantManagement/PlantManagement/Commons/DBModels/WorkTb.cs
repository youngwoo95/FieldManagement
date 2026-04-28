using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

/// <summary>
/// 작업 테이블
/// </summary>
public partial class WorkTb
{
    /// <summary>
    /// 작업지시번호 PK
    /// </summary>
    public string WorkSeq { get; set; } = null!;

    /// <summary>
    /// 수주 FK
    /// </summary>
    public int OrderSeq { get; set; }

    /// <summary>
    /// 설비 FK
    /// </summary>
    public int FacilitySeq { get; set; }

    /// <summary>
    /// 현재수량
    /// </summary>
    public int CurrentQty { get; set; }

    /// <summary>
    /// 작업 시작일
    /// </summary>
    public DateTime? StartWorkDt { get; set; }

    /// <summary>
    /// 작업 종료일
    /// </summary>
    public DateTime? EndWorkDt { get; set; }

    /// <summary>
    /// 현재 상태
    /// </summary>
    public int Status { get; set; }

    public bool DelYn { get; set; }

    public virtual ICollection<FacilityLogTb> FacilityLogTbs { get; set; } = new List<FacilityLogTb>();

    public virtual FacilityTb FacilitySeqNavigation { get; set; } = null!;

    public virtual OrderTb OrderSeqNavigation { get; set; } = null!;

    public virtual ICollection<ProductionTb> ProductionTbs { get; set; } = new List<ProductionTb>();
}
