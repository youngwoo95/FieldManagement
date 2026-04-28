using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

/// <summary>
/// 층정보 테이블
/// </summary>
public partial class FloorTb
{
    /// <summary>
    /// PK
    /// </summary>
    public int FloorSeq { get; set; }

    /// <summary>
    /// 층이름
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// FK
    /// </summary>
    public int FacilitySeq { get; set; }

    /// <summary>
    /// 첨부파일
    /// </summary>
    public string? Attach { get; set; }

    public virtual FacilityTb FacilitySeqNavigation { get; set; } = null!;
}
