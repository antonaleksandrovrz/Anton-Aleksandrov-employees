using Anton_Aleksandrov_employees.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton_Aleksandrov_employees.Helpers
{
    class EmployeeHelper
    {
        // Helper class for working with employee data
        private List<Employee> employees;
        private string dateFormat;

        // Constructor to initialize EmployeeHelper with employee data and date format
        public EmployeeHelper(List<Employee> employees, string dateFormat)
        {
            if (employees == null)
                throw new ArgumentNullException(nameof(employees), "Employees list cannot be null.");

            if (string.IsNullOrEmpty(dateFormat))
                throw new ArgumentException("Date format cannot be null or empty.", nameof(dateFormat));

            this.employees = employees;
            this.dateFormat = dateFormat;
        }

        // Get a list of EmployeePair objects with the most worked time together
        public List<EmployeePair> GetEmployeePairsWithMostWorkedTimeTogether()
        {
            // Get all employee pairs and find the pair with the most worked time together
            List<EmployeePair> employeePairs = GetEmployeePairs();
            EmployeePair topEmployeePair = employeePairs.OrderByDescending(empPair => empPair.DaysWorkedTogether).FirstOrDefault();

            // Filter the employee pairs to include only those with the same pair of employee IDs as the top pair
            List<EmployeePair> pairsWithMostWorkedTimeTogether = employeePairs
                .Where(empPair =>
                    (empPair.EmpID_First == topEmployeePair.EmpID_First && empPair.EmpID_Second == topEmployeePair.EmpID_Second) ||
                    (empPair.EmpID_Second == topEmployeePair.EmpID_First && empPair.EmpID_First == topEmployeePair.EmpID_Second)
                )
                .ToList();

            return pairsWithMostWorkedTimeTogether;
        }

        // Get a list of all employee pairs with positive worked time together
        private List<EmployeePair> GetEmployeePairs()
        {
            return (
                from emp1 in employees
                from emp2 in employees
                    // Ensure emp1's ID is less than emp2's ID to avoid duplicate pairs
                where emp1.EmpID < emp2.EmpID && emp1.ProjectID == emp2.ProjectID && DateHelper.CalculateDaysWorkedTogether(emp1, emp2, dateFormat) > 0
                select new EmployeePair
                {
                    EmpID_First = emp1.EmpID,
                    EmpID_Second = emp2.EmpID,
                    ProjectID = emp1.ProjectID,
                    DaysWorkedTogether = DateHelper.CalculateDaysWorkedTogether(emp1, emp2, dateFormat)
                }
            ).Distinct().ToList();
        }
    }
}
