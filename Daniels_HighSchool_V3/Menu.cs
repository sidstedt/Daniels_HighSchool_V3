using Daniels_HighSchool_V3.Database;
using Daniels_HighSchool_V3.Utils;

namespace Daniels_HighSchool_V3
{
    public class Menu
    {
        private bool run = true;
        private int selectedIndex = 0;
        private readonly EFHandler _efHandler;
        private readonly ADOHandler _adoHandler;
        private readonly Validator _validator;

        public Menu()
        {
            _efHandler = new EFHandler();
            _adoHandler = new ADOHandler();
            _validator = new Validator();
        }

        private string mainTitle = "Välkommen till Daniels Highschool - Administrationssystem\n" +
            "Välj ett av följande alternativ med piltangeterna och tryck på enter.";
        private string[] mainMenuOptions =
        {
            "1. Personalhantering",
            "2. Elevhantering",
            "3. Kurser & Betyg",
            "4. Ekonomi",
            "5. Avsluta"
        };
        private string subTitle = "Välj ett av följande alternativ med piltangeterna och tryck på enter.";
        private string[] staffMenuOptions =
        {
            "1. Visa antal anställda per avdelning",
            "2. Visa all personal",
            "3. Lägg till ny personal",
            "4. Tillbaka"
        };
        private string[] studentMenuOptions =
        {
            "1. Visa alla elever",
            "2. Visa en elevs information, via ID",
            "3. Uppdatera en elevs information",
            "4. Visa alla betyg för en elev",
            "5. Tillbaka"
        };
        private string[] courseMenuOptions =
        {
            "1. Visa aktiva kurser",
            "2. Visa en elevs betyg",
            "3. Visa alla ämnen och vilka lärare som undervisar i dem",
            "4. Sätt betyg på en elev",
            "5. Tillbaka"
        };
        private string[] financeMenuOptions =
        {
            "1. Visa totala lön per avdelning",
            "2. Visa medellön per avdelning",
            "3. Tillbaka"
        };

        public int MenuHandler(string title, string[] options)
        {
            selectedIndex = 0;
            while (run)
            {
                ConsoleKey keyPressed;
                do
                {
                    Console.Clear();
                    Console.WriteLine(title);
                    for (int i = 0; i < options.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{options[i]} <--");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"{options[i]}");
                        }
                    }

                    var keyInfo = Console.ReadKey(true);
                    keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = options.Length - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= options.Length)
                        {
                            selectedIndex = 0;
                        }
                    }
                }
                while (keyPressed != ConsoleKey.Enter);
                return selectedIndex;
            }
            return selectedIndex;
        }

        public void ShowMainMenu()
        {
            while (run)
            {
                var index = MenuHandler(mainTitle, mainMenuOptions);
                Console.Clear();
                switch (index)
                {
                    case 0:
                        ShowStaffMenu();
                        break;
                    case 1:
                        ShowStudentMenu();
                        break;
                    case 2:
                        ShowCourseMenu();
                        break;
                    case 3:
                        ShowFinanceMenu();
                        break;
                    case 4:
                        Console.WriteLine("Avslutar programmet...");
                        run = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val.");
                        break;
                }
                if (run)
                {
                    Console.WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
                    Console.ReadKey();
                }
            }
        }
        public void ShowStaffMenu()
        {
            var index = MenuHandler(subTitle, staffMenuOptions);
            Console.Clear();
            switch (index)
            {
                case 0:
                    ShowStaffCountByDepartment();
                    break;
                case 1:
                    ShowAllStaff();
                    break;
                case 2:
                    AddNewStaff();
                    break;
                case 3:
                    return;
            }
        }
        public void ShowStudentMenu()
        {
            var index = MenuHandler(subTitle, studentMenuOptions);
            Console.Clear();
            switch (index)
            {
                case 0:
                    ShowAllStudents();
                    break;
                case 1:
                    ShowStudentInfoById_SP();
                    break;
                case 2:
                    UpdateStudentInfo();
                    break;
                case 3:
                    ShowStudentGrades();
                    break;
                case 4:
                    return;

            }
        }
        public void ShowCourseMenu()
        {
            var index = MenuHandler(subTitle, courseMenuOptions);
            Console.Clear();
            switch (index)
            {
                case 0:
                    ShowActiveCourses();
                    break;
                case 1:
                    ShowStudentCourseDetails();
                    break;
                case 2:
                    ShowTeacherSubjects();
                    break;
                case 3:
                    AssignGradeToStudent();
                    break;
                case 4:
                    return;
            }
        }
        public void ShowFinanceMenu()
        {
            var index = MenuHandler(subTitle, financeMenuOptions);
            Console.Clear();
            switch (index)
            {
                case 0:
                    ShowDepartmentSalaryCost();
                    break;
                case 1:
                    ShowDepartmentAverageSalary();
                    break;
                case 2:
                    return;
            }
        }
        public void ShowStaffCountByDepartment()
        {
            var results = _efHandler.GetStaffCountByDepartment();
            Console.WriteLine("Antal anställda per avdelning:\n");
            foreach (var result in results)
            {
                Console.WriteLine($"{result.DepartmentName}: {result.StaffCount}");
            }
        }
        public void ShowAllStudents()
        {
            var students = _efHandler.GetAllStudents()
                .OrderBy(s => s.ClassName)
                .ThenBy(s => s.StudentName)
                .ToList();

            Console.WriteLine($"{"Namn".PadRight(20)} {"Personnr".PadRight(20)} {"Klass".PadRight(10)} Inskrivningsdatum");
            Console.WriteLine(new string('-', 70));

            string previousClass = "";

            foreach (var student in students)
            {
                if (student.ClassName != previousClass && previousClass != "")
                {
                    Console.WriteLine(new string('-', 70));
                }
                previousClass = student.ClassName;

                Console.WriteLine(
                    $"{student.StudentName.PadRight(20)} " +
                    $"{student.PersonalNumber.PadRight(20)} " +
                    $"{student.ClassName.PadRight(10)} " +
                    $"{student.StartDate:yyyy-MM-dd}");
            }
        }
        public void ShowActiveCourses()
        {
            var courses = _efHandler.GetActiveSubjects();

            Console.WriteLine("Aktiva kurser:\n");
            Console.WriteLine($"{"Ämne".PadRight(20)} {"Klass".PadRight(10)} Termin");
            Console.WriteLine(new string('-', 40));

            foreach (var course in courses)
            {
                Console.WriteLine($"{course.SubjectName.PadRight(20)} {course.ClassName.PadRight(10)} {course.Semester}");
            }
        }
        public void ShowAllStaff()
        {
            var staff = _adoHandler.GetAllStaff();

            Console.Clear();
            Console.WriteLine("Översikt över all personal:\n");
            Console.WriteLine($"{"Namn".PadRight(25)} {"Befattning".PadRight(20)} {"Antal år i tjänst"}");
            Console.WriteLine(new string('-', 60));

            foreach (var person in staff)
            {
                Console.WriteLine(
                    $"{person.Name.PadRight(25)} {person.PositionName.PadRight(20)} {person.YearsWorked} år");
            }
        }
        public void AddNewStaff()
        {
            try
            {
                string firstName = _validator.GetValidName("Ange förnamn:");
                string lastName = _validator.GetValidName("Ange efternamn:");

                var positions = _efHandler.GetAllPositions();
                if (!positions.Any())
                {
                    Console.WriteLine("Inga positioner tillgängliga. Kontakta administratör.");
                    return;
                }
                int positionId = _validator.GetValidatedInput
                    (
                        "Ange positions-ID:", positions.Select(p => p.PositionId).ToList(),
                        id => positions.First(p => p.PositionId == id).PositionName
                    );

                string personalNumber = _validator.GetValidPersonalNumber("Ange personnummer (12 tecken):");
                DateOnly employmenyStart = _validator.GetValidDate("Ange anställningsdatum (ÅÅÅÅ-MM-DD): ");
                decimal monthlySalary = _validator.GetValidDecimal("Ange månadslön: ");

                _adoHandler.AddNewStaff(firstName, lastName, positionId, employmenyStart, monthlySalary, personalNumber);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ett fel inträffade vid tillägg av personal. Försök igen.");
                Console.WriteLine($"Felmeddelande: {ex.Message}");
            }
        }
        public void ShowStudentGrades()
        {
            Console.Clear();
            Console.WriteLine("Välj en elev för att visa alla betyg:");

            var students = _adoHandler.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("Inga elever hittades.");
                return;
            }
            string[] studentOptions = students.Select(s => $"{s.StudentName} ({s.ClassName})").ToArray();
            int selectedIndex = MenuHandler("Välj en elev:", studentOptions);

            var selectedStudent = students[selectedIndex];

            var grades = _adoHandler.GetGradesByStudentId(selectedStudent.StudentId);
            if (!grades.Any())
            {
                Console.WriteLine("Eleven har inga betyg ännu.");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Betyg för: {selectedStudent.StudentName}");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"Ämne".PadRight(28)} {"Betyg".PadRight(8)} {"Lärare".PadRight(20)} {"Datum"}");
            Console.WriteLine(new string('-', 70));

            foreach (var grade in grades)
            {
                Console.WriteLine($"{grade.SubjectName.PadRight(28)} {grade.Grade.PadRight(8)} {grade.TeacherName.PadRight(20)} {grade.GradeDate}");
            }
        }

        public void ShowStudentCourseDetails()
        {
            Console.Clear();
            Console.WriteLine("Välj en elev för att visa betyg i en specifik kurs:");

            var students = _adoHandler.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("Inga elever hittades.");
                return;
            }

            int selectedIndex = MenuHandler("Välj en elev:",
                students.Select(s => $"{s.StudentName} ({s.ClassName})").ToArray());

            var selectedStudent = students[selectedIndex];

            var subjects = _adoHandler.GetInactiveSubjectsByStudent(selectedStudent.StudentId);
            if (!subjects.Any())
            {
                Console.WriteLine("Eleven har inga betyg ännu.");
                return;
            }

            int selectedSubjectIndex = MenuHandler("Välj ett ämne att visa betyg i:",
                subjects.Select(s => s.SubjectName).ToArray());

            var selectedSubject = subjects[selectedSubjectIndex];

            var mentor = _adoHandler.GetMentorByStudentId(selectedStudent.StudentId);

            var grades = _adoHandler.GetGradesByStudentIdAndSubject(selectedStudent.StudentId, selectedSubject.SubjectId);

            Console.Clear();
            Console.WriteLine($"Elev: {selectedStudent.StudentName}");
            Console.WriteLine($"Klass: {selectedStudent.ClassName}");
            Console.WriteLine($"Mentor: {mentor.Name}");
            Console.WriteLine(new string('-', 60));

            Console.WriteLine($"Betyg för ämnet: {selectedSubject.SubjectName}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"{"Ämne",-20} {"Betyg",-5} {"Lärare",-20} {"Datum"}");
            Console.WriteLine(new string('-', 60));

            foreach (var grade in grades)
            {
                Console.WriteLine($"{grade.SubjectName,-20} {grade.Grade,-5} {grade.TeacherName,-20} {grade.GradeDate}");
            }
        }
        public void ShowDepartmentSalaryCost()
        {
            var salaryCosts = _adoHandler.GetDepartmentSalaryCost();
            Console.Clear();
            Console.WriteLine("Översikt över totala lönekostnader per avdelning:\n");
            Console.WriteLine($"{"Avdelning".PadRight(20)} Lönekostnader");

            foreach (var salaryCost in salaryCosts)
            {
                Console.WriteLine($"{salaryCost.DepartmentName.PadRight(20)} {salaryCost.Salary:C2}");
            }
        }
        public void ShowDepartmentAverageSalary()
        {
            var salaryCosts = _adoHandler.GetDepartmentAverageSalary();
            Console.Clear();
            Console.WriteLine("Översikt över medellönen per avdelning:\n");
            Console.WriteLine($"{"Avdelning".PadRight(20)} Lönekostnader");

            foreach (var salaryCost in salaryCosts)
            {
                Console.WriteLine($"{salaryCost.DepartmentName.PadRight(20)} {salaryCost.Salary:C2}");
            }
        }
        public void ShowStudentInfoById_SP()
        {
            Console.Clear();
            Console.WriteLine("Välj en elev för att visa detaljer:");

            var students = _adoHandler.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("Inga elever hittades.");
                return;
            }

            int selectedIndex = MenuHandler("Välj en elev:",
                students.Select(s => $"{s.StudentName} ({s.ClassName})").ToArray());

            var selectedStudent = students[selectedIndex];

            var studentInfo = _adoHandler.GetStudentById_SP(selectedStudent.StudentId);

            Console.Clear();
            Console.WriteLine($"Elevinformation:");
            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Namn: {studentInfo.StudentName}");
            Console.WriteLine($"Personnr: {studentInfo.PersonalNumber}");
            Console.WriteLine($"Klass: {studentInfo.ClassName}");
            Console.WriteLine($"Startdatum: {studentInfo.StartDate}");
            Console.WriteLine(new string('-', 50));
        }

        public void AssignGradeToStudent()
        {
            var students = _adoHandler.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("Inga elever hittades.");
                return;
            }

            int studentIndex = MenuHandler("Välj en elev:",
                students.Select(s => $"{s.StudentName} ({s.ClassName})").ToArray());
            var selectedStudent = students[studentIndex];

            Console.Clear();
            Console.WriteLine("Välj ett ämne:");

            var subjects = _adoHandler.GetActiveSubjectsByStudent(selectedStudent.StudentId);
            if (!subjects.Any())
            {
                Console.WriteLine("Inga aktiva ämnen hittades.");
                return;
            }

            int subjectIndex = MenuHandler("Välj ett ämne:",
                subjects.Select(s => s.SubjectName).ToArray());
            var selectedSubject = subjects[subjectIndex];

            var teacher = _adoHandler.GetTeacherBySubject(selectedSubject.SubjectId);
            if (teacher == null)
            {
                Console.WriteLine($"Ingen lärare hittades för ämnet {selectedSubject.SubjectName}.");
                return;
            }

            Console.Clear();
            Console.WriteLine("Ange betyg (A-F):");
            string grade = Console.ReadLine().ToUpper();

            if (!new[] { "A", "B", "C", "D", "E", "F" }.Contains(grade))
            {
                Console.WriteLine("Ogiltigt betyg.");
                return;
            }

            bool success = _adoHandler.AssignGrade(selectedStudent.StudentId, selectedSubject.SubjectId, teacher.StaffId, grade);

            if (success)
            {
                Console.WriteLine($"Betyget {grade} har registrerats för {selectedStudent.StudentName} i ämnet {selectedSubject.SubjectName}.");
            }
            else
            {
                Console.WriteLine("Misslyckades att registrera betyget.");
            }
        }

        public void UpdateStudentInfo()
        {
            var students = _efHandler.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("Inga elever hittades.");
                return;
            }
            string[] studentOptions = students.Select(s => $"{s.StudentName} ({s.ClassName})").ToArray();

            int studentIndex = MenuHandler("Välj en elev:", studentOptions);

            int selectedStudentId = students[studentIndex].StudentId;

            var selectedStudent = students.FirstOrDefault(s => s.StudentId == selectedStudentId);

            bool run = true;
            while (run)
            {
                Console.Clear();
                Console.WriteLine($"Uppdatera information för: {selectedStudent.StudentName} ({selectedStudent.ClassName})");
                Console.WriteLine("Välj vad du vill uppdatera:");
                string[] updateOptions = { "Namn", "Personnummer", "Startdatum", "Klass", "Avsluta" };
                int choice = MenuHandler("Välj en uppgift att uppdatera:", updateOptions);

                switch (choice)
                {
                    case 0:
                        string newName = _validator.GetValidName("Ange nytt namn:");
                        _efHandler.UpdateStudentName(selectedStudent.StudentId, newName);
                        break;
                    case 1:
                        string newPersonalNumber = _validator.GetValidPersonalNumber("Ange nytt personnummer:");
                        _efHandler.UpdateStudentPersonalNumber(selectedStudent.StudentId, newPersonalNumber);
                        break;
                    case 2:
                        DateOnly newStartDate = _validator.GetValidDate("Ange nytt startdatum (ÅÅÅÅ-MM-DD):");
                        _efHandler.UpdateStudentStartDate(selectedStudent.StudentId, newStartDate);
                        break;
                    case 3:
                        var classes = _efHandler.GetAllClasses();
                        int classIndex = MenuHandler("Välj en ny klass:", classes.Select(c => c.ClassName).ToArray());
                        _efHandler.UpdateStudentClass(selectedStudent.StudentId, classes[classIndex].ClassId);
                        break;
                    case 4: // Avsluta
                        run = false;
                        return;
                }
                run = false;
                Console.WriteLine("Uppdatering slutförd! Tryck på valfri tangent för att fortsätta.");
                Console.ReadKey();
            }
        }
        public void ShowTeacherSubjects()
        {
            var teacherSubjects = _adoHandler.GetTeacherSubjects();

            Console.Clear();
            Console.WriteLine("Lärare och deras ämnen:\n");
            Console.WriteLine($"{"Lärarens namn",-25} {"Ämne"}");
            Console.WriteLine(new string('-', 50));

            foreach (var ts in teacherSubjects)
            {
                Console.WriteLine($"{ts.TeacherName.PadRight(25)} {ts.SubjectName}");
            }
        }

    }
}
