using System;
using System.Collections.Generic;

namespace Daniels_HighSchool_V3.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int? MentorId { get; set; }

    public virtual ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();

    public virtual StaffOverview? Mentor { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
