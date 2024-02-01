using Anton_Aleksandrov_employees.Data.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton_Aleksandrov_employees.Helpers
{
    public static class DateHelper
    {
        public static readonly string[] PossibleDateFormats = new string[] { "yy-MM-dd", "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy", "dd.MM.yyyy", "yy-MM-dd", };

        // Determine the date format used in a list of date strings
        public static string DetermineDateFormat(List<string> dateStrings)
        {
            foreach (var format in PossibleDateFormats)
            {
                if (AreAllDatesValid(dateStrings, format))
                {
                    return format; // Return the first format that validates all date strings
                }
            }

            return "yyyy-MM-dd"; // Default format if none of the formats match
        }

        // Check if all date strings in the list are valid for a given format
        private static bool AreAllDatesValid(List<string> dateStrings, string format)
        {
            return dateStrings.All(dateString => DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
        }

        // Calculate the number of days two employees worked together based on date format
        public static int CalculateDaysWorkedTogether(Employee emp1, Employee emp2, string dateFormat)
        {
            // Convert date strings to DateTime objects
            DateTime dateFromEmp1 = ConvertDataTime(emp1.DateFrom, false, dateFormat);
            DateTime dateToEmp1 = ConvertDataTime(emp1.DateTo, true, dateFormat);

            DateTime dateFromEmp2 = ConvertDataTime(emp2.DateFrom, false, dateFormat);
            DateTime dateToEmp2 = ConvertDataTime(emp2.DateTo, true, dateFormat);

            // Calculate the overlap period
            DateTime overlapStart = dateFromEmp1 > dateFromEmp2 ? dateFromEmp1 : dateFromEmp2;
            DateTime overlapEnd = dateToEmp1 < dateToEmp2 ? dateToEmp1 : dateToEmp2;

            // Calculate the days worked together
            int daysWorkedTogether = (overlapEnd.Date - overlapStart.Date).Days;
            return daysWorkedTogether >= 0 ? daysWorkedTogether : 0; // Ensure non-negative result
        }

        // Convert a date string to a DateTime object, handling null values
        public static DateTime ConvertDataTime(string date, bool canBeNull, string dateFormat)
        {
            DateTime parsedDate;

            // Handle null values
            if (canBeNull && string.Equals(date, "null", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today; // Return today's date for null values
            }

            // Parse the date string using the specified format
            if (DateTime.TryParseExact(date, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate; // Return the parsed date
            }
            else
            {
                throw new Exception($"DateTime can't be parsed because it has value: {date}");
            }
        }

        // Generate a random date within a range starting from a given date
        public static DateTime RandomDate(Random random, DateTime startDate)
        {
            DateTime endDate = startDate.AddMonths(random.Next(1, 12)).AddDays(random.Next(1, 30)).AddYears(random.Next(3, 10));
            return startDate.AddDays(random.Next((int)(endDate - startDate).TotalDays));
        }
    }
}
