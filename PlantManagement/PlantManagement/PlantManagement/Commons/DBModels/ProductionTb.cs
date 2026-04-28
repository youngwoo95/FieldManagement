using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

/// <summary>
/// 생산 테이블
/// </summary>
public partial class ProductionTb
{
    public int ProductionSeq { get; set; }

    /// <summary>
    /// 작업지시번호
    /// </summary>
    public string WorkerSeq { get; set; } = null!;

    /// <summary>
    /// 제품상태 1: 정상 0:불량
    /// </summary>
    public bool Status { get; set; }

    public DateTime CreateDt { get; set; }

    public virtual WorkTb WorkerSeqNavigation { get; set; } = null!;
}
