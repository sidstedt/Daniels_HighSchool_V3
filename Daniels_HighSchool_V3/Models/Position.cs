using System;
using System.Collections.Generic;

namespace Daniels_HighSchool_V3.Models;

public partial class Position
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<StaffOverview> Staff { get; set; } = new List<StaffOverview>();
}
