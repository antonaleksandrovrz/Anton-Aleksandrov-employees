using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Anton_Aleksandrov_employees.Data.Models;
using CsvHelper.Configuration;
using CsvHelper;

namespace Anton_Aleksandrov_employees.Helpers
{
    class EmployeeHelper
    {
        public static List<Employee> LoadEmployeesFromCsv()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                if (System.IO.Path.GetExtension(selectedFilePath).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        TrimOptions = TrimOptions.Trim
                    };

                    using (var reader = new StreamReader(selectedFilePath))
                    using (var csv = new CsvReader(reader, configuration))
                    {
                        try
                        {
                            return csv.GetRecords<Employee>().ToList();
                        }
                        catch (CsvHelper.HeaderValidationException ex)
                        {
                            throw new Exception("Unrecognized header format. Please ensure the CSV file has the correct headers.");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("An error occurred while processing the CSV file.", ex);
                        }
                    }
                }
            }

            throw new Exception("Invalid file type. Please select a CSV file.");
        }

        public static void GenerateEmployeesCsvFile(string dateFormat,int maxRows)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = saveFileDialog.FileName;

                List<Employee> data = GenerateRandomData(maxRows, dateFormat);

                using (var writer = new StreamWriter(selectedFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data);
                }

                MessageBox.Show("Random CSV file generated successfully!");
            }
        }

        private static List<Employee> GenerateRandomData(int rowCount, string dateFormat)
        {
            List<Employee> data = new List<Employee>();
            Random random = new Random();

            for (int i = 0; i < rowCount; i++)
            {
                DateTime dateFrom = DateHelper.RandomDate(random, new DateTime(2000, 1, 1));

                Employee row = new Employee
                {
                    EmpID = random.Next(100, 1000),
                    ProjectID = random.Next(1, 20),
                    DateFrom = dateFrom.ToString(dateFormat),
                    DateTo = random.Next(2) == 0 ? DateHelper.RandomDate(random, dateFrom).ToString(dateFormat) : "NULL"
                };

                data.Add(row);
            }

            return data;
        }

        public static List<EmployeePair> GetEmployeePairs(List<Employee> employees, string dateFormat)
        {
            return (
                from emp1 in employees
                from emp2 in employees
                where emp1.EmpID < emp2.EmpID && emp1.ProjectID == emp2.ProjectID && DateHelper.CalculateDaysWorkedTogether(emp1, emp2, dateFormat) > 0
                select new EmployeePair
                {
                    EmpID_First = emp1.EmpID,
                    EmpID_Second = emp2.EmpID,
                    ProjectID = emp1.ProjectID,
                    DaysWorkedTogether = DateHelper.CalculateDaysWorkedTogether(emp1, emp2, dateFormat)
                }
            ).ToList();
        }
    }
}
