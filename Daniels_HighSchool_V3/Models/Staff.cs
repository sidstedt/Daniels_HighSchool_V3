using System;
using System.Collections.Generic;

namespace Daniels_HighSchool_V3.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int PositionId { get; set; }

    public DateOnly EmploymentStartDate { get; set; }

    public decimal MonthlySalary { get; set; }

    public string PersonalNumber { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Position Position { get; set; } = null!;

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
