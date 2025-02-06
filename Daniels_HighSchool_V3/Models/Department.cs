using System;
using System.Collections.Generic;

namespace Daniels_HighSchool_V3.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
