using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3.DTOs
{
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateOnly StartDate { get; set; }
        public string PersonalNumber { get; set; }
        public string ClassName { get; set; }
    }
}
