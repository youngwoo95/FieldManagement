using System;
using System.Collections.Generic;

namespace PlantManagement.Commons.DBModels;

public partial class FloorLampTb
{
    public int LampSeq { get; set; }

    public int FloorSeq { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public virtual FloorTb FloorSeqNavigation { get; set; } = null!;

    public virtual LampTb LampSeqNavigation { get; set; } = null!;
}
