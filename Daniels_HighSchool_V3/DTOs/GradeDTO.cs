using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3.DTOs
{
    public class GradeDTO
    {
        public string Grade { get; set; }
        public DateOnly GradeDate { get; set; }
        public string TeacherName { get; set; }
        public string SubjectName { get; set; }
    }
}
