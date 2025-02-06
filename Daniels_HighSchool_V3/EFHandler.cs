using Daniels_HighSchool_V3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3
{
    internal class EFHandler
    {
        private readonly DanielsHighschoolDbV3Context _context;

        public EFHandler()
        {
            _context = new DanielsHighschoolDbV3Context();
        }
        public int GetTeacherCountByDepartment()
        {

        }
        public List<Student> GetAllStudents()
        {

        }
        public List<Subject> GetActiveSubjects()
        {

        }
    }
}
