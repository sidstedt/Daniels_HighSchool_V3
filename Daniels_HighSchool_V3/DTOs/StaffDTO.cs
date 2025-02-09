using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3.DTOs
{
    public class StaffDTO
    {
        public int StaffId { get; set; }
        public string Name { get; set; }
        public string PositionName { get; set; }
        public int YearsWorked { get; set; }
    }
}
