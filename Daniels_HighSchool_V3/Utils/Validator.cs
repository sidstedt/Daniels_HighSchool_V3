using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniels_HighSchool_V3.Utils
{
    internal class Validator
    {
        public int GetValidatedInput(string prompt, List<int> validOptions, Func<int, string> displayOption = null)
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("Tillgängliga alternativ:");

                foreach (var option in validOptions)
                {
                    if (displayOption != null)
                    {
                        Console.WriteLine($"{option}: {displayOption(option)}");
                    }
                    else
                    {
                        Console.WriteLine(option);
                    }
                }

                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (int.TryParse(input, out choice) && validOptions.Contains(choice))
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine("Ogiltligt val. Tryck på enter för att försöka igen.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
            } while (true);
        }

        public string GetValidName(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input) && input.Length >= 2 && input.All(c => char.IsLetter(c) || c == '-'))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Namnet får endast innehålla bokstäver, bindestreck och måste vara minst två bokstäver långt.\n" +
                        "Tryck på enter för att försök igen.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                }
            }
        }
        public string GetValidPersonalNumber(string prompt)
        {
            string birthDate, lastFour;

            while (true)
            {
                Console.WriteLine($"{prompt} (Ange födelsedatum YYYYMMDD):");
                birthDate = Console.ReadLine();

                if (birthDate.Length == 8 && DateOnly.TryParseExact(birthDate, "yyyyMMdd", out _))
                {
                    break;
                }
                Console.WriteLine("Felaktigt format! Ange ett giltigt födelsedatum i formatet YYYYMMDD.");
            }

            while (true)
            {
                Console.WriteLine("Ange de sista fyra siffrorna (XXXX):");
                lastFour = Console.ReadLine();

                if (lastFour.Length == 4 && lastFour.All(char.IsDigit))
                {
                    break;
                }
                Console.WriteLine("Felaktigt format! De sista fyra måste vara exakt fyra siffror.");
            }

            return $"{birthDate}-{lastFour}";
        }
        public DateOnly GetValidDate(string prompt)
        {
            const int maxMonthsAhead = 3;
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly maxAllowedDate = today.AddMonths(maxMonthsAhead);
            DateOnly validDate;

            while (true)
            {
                Console.WriteLine($"{prompt} (Format: YYYY-MM-DD):");
                string input = Console.ReadLine();

                if (DateOnly.TryParseExact(input, "yyyy-MM-dd", out validDate))
                {
                    if (validDate > maxAllowedDate)
                    {
                        Console.WriteLine($"Felaktigt datum! Datum får vara max {maxMonthsAhead} månader framåt.");
                        continue;
                    }

                    return validDate;
                }
                else
                {
                    Console.WriteLine("Felaktigt format! Ange datum i formatet YYYY-MM-DD.");
                }
            }
        }

        public decimal GetValidDecimal(string prompt)
        {
            decimal validNumber;
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out validNumber) && validNumber >= 0)
                {
                    return validNumber;
                }
                else
                {
                    Console.WriteLine("Felaktig inmatning! Ange ett giltigt numeriskt värde (ex. 45000.50).");
                }
            }
        }
    }
}
