using Anton_Aleksandrov_employees.Data.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton_Aleksandrov_employees.Data
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            // Map CSV fields to Employee class properties
            Map(m => m.EmpID).Name("EmpID").Index(0).Validate(field => ValidateInteger(field.Field));
            Map(m => m.ProjectID).Name("ProjectID").Index(1).Validate(field => ValidateInteger(field.Field));
            Map(m => m.DateFrom).Name("DateFrom").Index(2).Validate(field => ValidateNotEmpty(field.Field));
            Map(m => m.DateTo).Name("DateTo").Index(3).Validate(field => ValidateNotEmpty(field.Field));
        }

        private bool ValidateInteger(string field)
        {
            return int.TryParse(field, out _);
        }

        // Validate that the field is not empty
        private bool ValidateNotEmpty(string field)
        {
            //Because the sample data doesn't specify whether it's a string or DateTime, I can't determine if a date like 01.02.2023 is January 2nd or February 1st before identifying the data format.
            return !string.IsNullOrWhiteSpace(field);
        }
    }
}
