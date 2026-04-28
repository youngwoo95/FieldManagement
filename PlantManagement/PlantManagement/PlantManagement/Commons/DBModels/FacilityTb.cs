using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

/// <summary>
/// 설비 테이블
/// </summary>
public partial class FacilityTb
{
    /// <summary>
    /// 설비 PK
    /// </summary>
    public int FacilitySeq { get; set; }

    /// <summary>
    /// 설비명
    /// </summary>
    public string FacilityName { get; set; } = null!;

    /// <summary>
    /// 제조사
    /// </summary>
    public string Maker { get; set; } = null!;

    /// <summary>
    /// 용도
    /// </summary>
    public string? Purpose { get; set; }

    /// <summary>
    /// 생성일
    /// </summary>
    public DateTime CreateTb { get; set; }

    /// <summary>
    /// 삭제여부
    /// </summary>
    public bool DelYn { get; set; }

    public virtual ICollection<FacilityLogTb> FacilityLogTbs { get; set; } = new List<FacilityLogTb>();

    public virtual ICollection<FloorTb> FloorTbs { get; set; } = new List<FloorTb>();

    public virtual ICollection<WorkTb> WorkTbs { get; set; } = new List<WorkTb>();
}
