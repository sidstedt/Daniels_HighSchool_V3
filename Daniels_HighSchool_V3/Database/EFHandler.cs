using Daniels_HighSchool_V3.Models;
using Microsoft.EntityFrameworkCore;
using Daniels_HighSchool_V3.DTOs;

namespace Daniels_HighSchool_V3.Database
{
    public class EFHandler
    {
        private readonly DanielsHighschoolDbV3Context _context;

        public EFHandler()
        {
            _context = new DanielsHighschoolDbV3Context();
        }
        public List<Class> GetAllClasses()
        {
            return _context.Classes.ToList();
        }
        public List<Position> GetAllPositions()
        {
            return _context.Positions.ToList();
        }

        public List<DepartmentStaffDTO> GetStaffCountByDepartment()
        {
            return _context.Staff
                .GroupBy(s => s.Position.Department.DepartmentName)
                .Select(group => new DepartmentStaffDTO
                {
                    DepartmentName = group.Key,
                    StaffCount = group.Count()
                })
                .ToList();
        }

        public List<StudentDTO> GetAllStudents()
        {
            return _context.Students
                .Include(c => c.Class)
                .Select(s => new StudentDTO
                {
                    StudentId = s.StudentId,
                    StudentName = s.FirstName + " " + s.LastName,
                    PersonalNumber = s.PersonalNumber,
                    StartDate = s.EnrollmentDate,
                    ClassName = s.Class != null ? s.Class.ClassName : "Ingen klass"
                })
                .ToList();
        }
        public List<ActiveCourseDTO> GetActiveSubjects()
        {
            return _context.ClassSubjects
                .Where(cs => cs.IsActive)
                .Include(cs => cs.Subject)
                .Include(cs => cs.Class)
                .Select(cs => new ActiveCourseDTO
                {
                    SubjectName = cs.Subject.SubjectName,
                    ClassName = cs.Class.ClassName,
                    Semester = cs.Semester
                })
                .ToList();
        }
        public void UpdateStudentName(int studentId, string newName)
        {
            var student = _context.Students.Find(studentId);
            if (student != null)
            {
                var nameParts = newName.Split(' ', 2);
                student.FirstName = nameParts[0];
                student.LastName = nameParts.Length > 1 ? nameParts[1] : "";
                _context.SaveChanges();
            }
        }
        public void UpdateStudentPersonalNumber(int studentId, string newPersonalNumber)
        {
            var student = _context.Students.Find(studentId);
            if (student != null)
            {
                student.PersonalNumber = newPersonalNumber;
                _context.SaveChanges();
            }
        }
        public void UpdateStudentStartDate(int studentId, DateOnly newStartDate)
        {
            var student = _context.Students.Find(studentId);
            if (student != null)
            {
                student.EnrollmentDate = newStartDate;
                _context.SaveChanges();
            }
        }
        public void UpdateStudentClass(int studentId, int newClassId)
        {
            var student = _context.Students.Find(studentId);
            if (student != null)
            {
                student.ClassId = newClassId;
                _context.SaveChanges();
            }
        }
    }
}
