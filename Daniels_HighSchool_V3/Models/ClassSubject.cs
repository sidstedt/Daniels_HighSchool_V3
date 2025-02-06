using System;
using System.Collections.Generic;

namespace Daniels_HighSchool_V3.Models;

public partial class ClassSubject
{
    public int ClassId { get; set; }

    public int SubjectId { get; set; }

    public bool IsActive { get; set; }

    public string Semester { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
