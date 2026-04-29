using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

public partial class FloorFacilityTb
{
    public int FloorSeq { get; set; }

    public int FacilitySeq { get; set; }

    public virtual FacilityTb FacilitySeqNavigation { get; set; } = null!;

    public virtual FloorTb FloorSeqNavigation { get; set; } = null!;
}
