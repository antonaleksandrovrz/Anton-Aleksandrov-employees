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
using Anton_Aleksandrov_employees.Data;

namespace Anton_Aleksandrov_employees.Helpers
{
    class EmployeeCsvHelper
    {
        public static List<Employee> LoadEmployeesFromCsv()
        {
            // Create an OpenFileDialog to allow the user to select a CSV file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";

            // If the user selects a file, process it
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                // Check if the selected file has a .csv extension
                if (System.IO.Path.GetExtension(selectedFilePath).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    // Configure CsvHelper to trim fields while reading
                    var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        TrimOptions = TrimOptions.Trim
                    };

                    // Use CsvHelper to read the CSV file and convert it to a list of Employee objects
                    using (var reader = new StreamReader(selectedFilePath))
                    using (var csv = new CsvReader(reader, configuration))
                    {
                        try
                        {
                            csv.Context.RegisterClassMap<EmployeeMap>();
                            
                            csv.Read();
                            csv.ReadHeader();
                            csv.ValidateHeader<Employee>();

                            return csv.GetRecords<Employee>().ToList(); // Return the list of employees
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

        // Generate a CSV file with random employee data
        public static void GenerateEmployeesCsvFile(string dateFormat, int maxRows)
        {
            // Create a SaveFileDialog to specify the location to save the generated CSV file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";

            // If the user specifies a location, generate and save the CSV file
            if (saveFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = saveFileDialog.FileName;

                // Generate random employee data
                List<Employee> data = GenerateRandomData(maxRows, dateFormat);

                // Use CsvHelper to write the data to a CSV file
                using (var writer = new StreamWriter(selectedFilePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data);
                }

                // Show a success message
                MessageBox.Show("Random CSV file generated successfully!");
            }
        }

        // Generate random employee data based on the specified parameters
        private static List<Employee> GenerateRandomData(int rowCount, string dateFormat)
        {
            List<Employee> data = new List<Employee>();
            Random random = new Random();

            // Generate random employee data for the specified number of rows
            for (int i = 0; i < rowCount; i++)
            {
                DateTime dateFrom = DateHelper.RandomDate(random, new DateTime(2000, 1, 1));

                // Create an Employee object with random values
                Employee row = new Employee
                {
                    EmpID = random.Next(100, 1000),
                    ProjectID = random.Next(1, 20),
                    DateFrom = dateFrom.ToString(dateFormat),
                    DateTo = random.Next(2) == 0 ? DateHelper.RandomDate(random, dateFrom).ToString(dateFormat) : "NULL"
                };

                // Add the employee data to the list
                data.Add(row);
            }

            return data; // Return the generated data
        }
    }
}
