using System;
using System.Collections.Generic;

namespace Daniels_HighSchool_V3.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual ICollection<StaffOverview> Staff { get; set; } = new List<StaffOverview>();
}
