using System;
using System.Collections.Generic;

namespace Daniels_HighSchool_V3.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public int StaffId { get; set; }

    public string Grade1 { get; set; } = null!;

    public DateOnly GradeDate { get; set; }

    public virtual StaffOverview Staff { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
