using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3
{
    internal class Menu
    {
        private bool run = true;
        private int selectedIndex = 0;
        private readonly EFHandler _efHandler;
        private readonly ADOHandler _adoHandler;

        public Menu()
        {
            _efHandler = new EFHandler();
            _adoHandler = new ADOHandler();
        }

        private string[] mainMenuOptions =
        {
            "1. Personalhantering",
            "2. Elevhantering",
            "3. Kurser & Betyg",
            "4. Ekonomi",
            "5. Avsluta"
        };
        private string[] staffMenuOptions =
        {
            "1. Visa antal personal per avdelning",
            "2. Visa all personal",
            "3. Lägg till ny personal",
            "4. Tillbaka"
        };
        private string[] studentMenuOptions =
        {
            "1. Visa alla elever",
            "2. Visa en specifik elev",
            "3. Uppdatera en elevs information",
            "4. Visa en elevs betyg i alla kurser",
            "5. Tillbaka"
        };

        public void ShowMainMenu()
        {
            while (run)
            {
                ConsoleKey keyPressed;
                do
                {
                    Console.Clear();
                    Console.WriteLine("Välj ett alternativ med piltangenterna och tryck ENTER:");
                    for (int i = 0; i < mainMenuOptions.Length; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{mainMenuOptions[i]} <--");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"{mainMenuOptions[i]}");
                        }
                    }

                    var keyInfo = Console.ReadKey(true);
                    keyPressed = keyInfo.Key;

                    if (keyPressed == ConsoleKey.UpArrow)
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = mainMenuOptions.Length - 1;
                        }
                    }
                    else if (keyPressed == ConsoleKey.DownArrow)
                    {
                        selectedIndex++;
                        if (selectedIndex >= mainMenuOptions.Length)
                        {
                            selectedIndex = 0;
                        }
                    }
                }
                while (keyPressed != ConsoleKey.Enter);
                RunMainOption(selectedIndex);
            }
        }
        public void RunMainOption(int index)
        {
            Console.Clear();
            switch (index)
            {
                case 0:
                    ShowStaffMenu();
                    break;
                case 1:
                    
                    break;
                case 2:
                    
                    break;
                case 3:
                    
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
        public void ShowStaffMenu()
        {

        }
        public void ShowTeacherCountByDepartment()
        {

        }
        public void ShowAllStudents()
        {

        }
        public void ShowActiveCourses()
        {

        }
        public void ShowStaffOverview()
        {

        }
        public void ShowStudentGrades()
        {

        }
        public void ShowDepartmentSalaryCost()
        {

        }
        public void ShowDepartmentAverageSalary()
        {

        }
        public void ShowStudentInfoById()
        {

        }
        public void AssignGradeToStudent()
        {

        }

    }
}
