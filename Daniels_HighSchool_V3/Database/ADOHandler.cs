using Daniels_HighSchool_V3.Models;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;
using Daniels_HighSchool_V3.Models;
using static Daniels_HighSchool_V3.Database.ADOHandler;
using Daniels_HighSchool_V3.DTOs;

namespace Daniels_HighSchool_V3.Database
{
    public class ADOHandler
    {
        private readonly DatabaseReader _reader;
        private readonly DatabaseWriter _writer;
        public ADOHandler()
        {
            _reader = new DatabaseReader();
            _writer = new DatabaseWriter();
        }

        public List<StaffDTO> GetAllStaff()
        {
            string query = @"
                SELECT 
                    s.FirstName, 
                    s.LastName, 
                    p.PositionName, 
                    DATEDIFF(YEAR, s.EmploymentStartDate, GETDATE()) AS YearsWorked
                FROM Staff s
                JOIN Positions p ON s.PositionId = p.PositionId
                ORDER BY YearsWorked DESC";
            return _reader.Read(query, r => new StaffDTO
            {
                Name = r.GetString(0) + " " + r.GetString(1),
                PositionName = r.GetString(2),
                YearsWorked = r.GetInt32(3)
            });
        }
        public void AddNewStaff(string firstName, string lastName, int positionId, DateOnly employmentStart, decimal monthlySalary, string personalNumber)
        {
            string query = @"
                INSERT INTO Staff (FirstName, LastName, PositionId, EmploymentStartDate, MonthlySalary, PersonalNumber)
                VALUES (@FirstName, @LastName, @PositionId, @EmploymentStartDate, @MonthlySalary, @PersonalNumber);";

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@FirstName", firstName),
                new SqlParameter("@LastName", lastName),
                new SqlParameter("@PositionId", positionId),
                new SqlParameter("@EmploymentStartDate", employmentStart.ToString("yyyy-MM-dd")),
                new SqlParameter("@MonthlySalary", monthlySalary),
                new SqlParameter("@PersonalNumber", personalNumber),
            };

            int rowsAffected = _writer.Write(query, parameters);

            if (rowsAffected > 0)
            {
                Console.WriteLine("Ny personal tillagd.");
            }
            else
            {
                Console.WriteLine("Misslyckades, ingen personal tillagd.");
            }
        }
        public List<GradeDTO> GetGradesByStudent(int studentId)
        {
            string query = @"
                SELECT 
                    g.Grade, g.GradeDate,
                    s.FirstName + ' ' + s.LastName AS TeacherName,
                    sub.SubjectName
                FROM Grades g
                JOIN Staff s ON g.StaffId = s.StaffId
                JOIN Subjects sub ON g.SubjectId = sub.SubjectId
                WHERE g.StudentId = @StudentId
                ORDER BY g.GradeDate DESC";

            return _reader.Read(query, r => new GradeDTO()
            {
                Grade = r.GetString(0),
                GradeDate = DateOnly.FromDateTime(r.GetDateTime(1)),
                TeacherName = r.GetString(2),
                SubjectName = r.GetString(3)
            }, new SqlParameter("@StudentId", studentId));
        }

        public List<GradeDTO> GetGradesByStudentId(int studentId)
        {
            string query = @"
                SELECT 
                    g.Grade, g.GradeDate,
                    s.FirstName + ' ' + s.LastName AS TeacherName,
                    sub.SubjectName
                FROM Grades g
                JOIN Staff s ON g.StaffId = s.StaffId
                JOIN Subjects sub ON g.SubjectId = sub.SubjectId
                WHERE g.StudentId = @StudentId
                ORDER BY g.GradeDate DESC";
            return _reader.Read(query, r => new GradeDTO
            {
                Grade = r.GetString(0),
                GradeDate = DateOnly.FromDateTime(r.GetDateTime(1)),
                TeacherName = r.GetString(2),
                SubjectName = r.GetString(3),
            }, new SqlParameter(@"StudentId", studentId));
        }
        public Student GetStudentById(int studentId)
        {
            string query = "EXEC GetStudentById @StudentId";

            var students = _reader.Read(query, r => new Student
            {
                StudentId = r.GetInt32(0),
                FirstName = r.GetString(1),
                LastName = r.GetString(2),
                PersonalNumber = r.GetString(3),
                EnrollmentDate = DateOnly.FromDateTime(r.GetDateTime(4)),
                ClassId = r.GetInt32(5)
            }, new SqlParameter("@StudentId", studentId));

            return students.FirstOrDefault();
        }
        public List<DTOs.StudentDTO> GetAllStudents()
        {
            string query = @"
                SELECT s.StudentId, s.FirstName + ' ' + s.LastName AS StudentName, c.ClassName
                FROM Students s
                JOIN Classes c ON s.ClassId = c.ClassId
                ORDER BY c.ClassName, s.LastName";

            return _reader.Read(query, r => new DTOs.StudentDTO
            {
                StudentId = r.GetInt32(0),
                StudentName = r.GetString(1),
                ClassName = r.GetString(2)
            });
        }
        public List<InactiveSubjectDTO> GetInactiveSubjectsByStudent(int studentId)
        {
            string query = @"
                SELECT DISTINCT cs.SubjectId, sub.SubjectName
                FROM Class_Subject cs
                JOIN Subjects sub ON cs.SubjectId = sub.SubjectId
                WHERE cs.IsActive = 0 
                AND cs.ClassId = (SELECT ClassId FROM Students WHERE StudentId = @StudentId)";

            return _reader.Read(query, r => new InactiveSubjectDTO
            {
                SubjectId = r.GetInt32(0),
                SubjectName = r.GetString(1)
            }, new SqlParameter("@StudentId", studentId));
        }
        public StaffDTO GetMentorByStudentId(int studentId)
        {
            string query = @"
                SELECT s.FirstName, s.LastName
                FROM Students st
                JOIN Classes c ON st.ClassId = c.ClassId
                JOIN Staff s ON c.MentorId = s.StaffId
                WHERE st.StudentId = @StudentId;";

            var result = _reader.Read(query, r => new StaffDTO
            {
                Name = r.GetString(0) + " " + r.GetString(1)
            }, new SqlParameter("@StudentId", studentId));

            return result.FirstOrDefault();
        }
        public List<GradeDTO> GetGradesByStudentIdAndSubject(int studentId, int subjectId)
        {
            string query = @"
                SELECT g.Grade, g.GradeDate,
                       s.FirstName + ' ' + s.LastName AS TeacherName, sub.SubjectName
                FROM Grades g
                JOIN Staff s ON g.StaffId = s.StaffId
                JOIN Subjects sub ON g.SubjectId = sub.SubjectId
                WHERE g.StudentId = @StudentId AND g.SubjectId = @SubjectId
                ORDER BY g.GradeDate DESC;";

            return _reader.Read(query, r => new GradeDTO
            {
                Grade = r.GetString(0),
                GradeDate = DateOnly.FromDateTime(r.GetDateTime(1)),
                TeacherName = r.GetString(2),
                SubjectName = r.GetString(3)
            },
            new SqlParameter("@StudentId", studentId),
            new SqlParameter("@SubjectId", subjectId)
            );
        }
        public List<DepartmentSalaryDTO> GetDepartmentSalaryCost()
        {
            string query = @"
                SELECT d.DepartmentName, SUM(s.MonthlySalary) AS TotalSalary
                FROM Departments d
                JOIN Positions p ON d.DepartmentId = p.DepartmentId
                JOIN Staff s ON p.PositionId = s.PositionId
                GROUP BY d.DepartmentName
                ORDER BY TotalSalary DESC";

            return _reader.Read(query, r => new DepartmentSalaryDTO
            {
                DepartmentName = r.GetString(0),
                Salary = r.GetDecimal(1)
            });
        }
        public List<DepartmentSalaryDTO> GetDepartmentAverageSalary()
        {
            string query = @"
                SELECT d.DepartmentName, AVG(s.MonthlySalary) AS AverageSalary
                FROM Departments d
                JOIN Positions p ON d.DepartmentId = p.DepartmentId
                JOIN Staff s ON p.PositionId = s.PositionId
                GROUP BY d.DepartmentName
                ORDER BY AverageSalary DESC";

            return _reader.Read(query, r => new DepartmentSalaryDTO
            {
                DepartmentName = r.GetString(0),
                Salary = r.GetDecimal(1)
            });

        }
        public StudentDTO GetStudentById_SP(int studentId)
        {
            string query = "EXEC GetStudentWithId @StudentId";

            var result = _reader.Read(query, r => new StudentDTO
            {
                StudentId = r.GetInt32(0),
                StudentName = r.GetString(1) + " " + r.GetString(2),
                PersonalNumber = r.GetString(3),
                StartDate = DateOnly.FromDateTime(r.GetDateTime(4)),
                ClassName = r.GetString(5)
            }, new SqlParameter("@StudentId", studentId));

            return result.FirstOrDefault();
        }
        public List<InactiveSubjectDTO> GetActiveSubjectsByStudent(int studentId)
        {
            string query = @"
                SELECT DISTINCT cs.SubjectId, sub.SubjectName
                FROM Class_Subject cs
                JOIN Subjects sub ON cs.SubjectId = sub.SubjectId
                WHERE cs.IsActive = 1 
                AND cs.ClassId = (SELECT ClassId FROM Students WHERE StudentId = @StudentId)";

            return _reader.Read(query, r => new InactiveSubjectDTO
            {
                SubjectId = r.GetInt32(0),
                SubjectName = r.GetString(1)
            }, new SqlParameter("@StudentId", studentId));
        }
        public StaffDTO GetTeacherBySubject(int subjectId)
        {
            string query = @"
                SELECT s.StaffId, s.FirstName, s.LastName 
                FROM Staff_Subject ss
                JOIN Staff s ON ss.StaffId = s.StaffId
                WHERE ss.SubjectId = @SubjectId
                ORDER BY s.EmploymentStartDate ASC;";

            var teachers = _reader.Read(query, r => new StaffDTO
            {
                StaffId = r.GetInt32(0),
                Name = r.GetString(1) + " " + r.GetString(2)
            }, new SqlParameter("@SubjectId", subjectId));

            return teachers.FirstOrDefault();
        }
        public bool AssignGrade(int studentId, int subjectId, int staffId, string grade)
        {
            string query = @"
                BEGIN TRANSACTION;
                BEGIN TRY
                    INSERT INTO Grades (StudentId, SubjectId, StaffId, Grade, GradeDate)
                    VALUES (@StudentId, @SubjectId, @StaffId, @Grade, GETDATE());
            
                    COMMIT TRANSACTION;
                    SELECT 1;
                END TRY
                BEGIN CATCH
                    ROLLBACK TRANSACTION;
                    SELECT 0;
                END CATCH";

            var parameters = new SqlParameter[]
                {
                    new SqlParameter("@StudentId", studentId),
                    new SqlParameter("@SubjectId", subjectId),
                    new SqlParameter("@StaffId", staffId),
                    new SqlParameter("@Grade", grade)
                };
            
            var result = _reader.Read(query, r => r.GetInt32(0), parameters);
            return result.FirstOrDefault() == 1;
        }
        public List<TeacherSubjectDTO> GetTeacherSubjects()
        {
            string query = "SELECT TeacherName, SubjectName FROM TeacherSubjects";

            return _reader.Read(query, r => new TeacherSubjectDTO
            {
                TeacherName = r.GetString(0),
                SubjectName = r.GetString(1)
            });
        }
    }
}
