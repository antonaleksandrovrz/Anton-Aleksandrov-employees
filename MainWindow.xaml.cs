using Anton_Aleksandrov_employees.Data.Models;
using Anton_Aleksandrov_employees.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Anton_Aleksandrov_employees
{
    public partial class MainWindow : Window
    {
        private List<Employee> employeesData = new List<Employee>();
        private string dateFormat = "yyyy-MM-dd";
        public MainWindow()
        {
            InitializeComponent();
            cBox_dateFormat.ItemsSource = DateHelper.PossibleDateFormats;
            cBox_dateFormat.SelectedValue = dateFormat;
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                employeesData = EmployeeHelper.LoadEmployeesFromCsv();

                if (employeesData != null && employeesData.Any())
                {
                    BindDataGrid(dGrid_LoadedData, employeesData);
                    BindDateFormat(employeesData);
                    FindEmployeePairs(employeesData);
                }

                else
                {
                    throw new Exception("Employees data is null or empty");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GenerateRandomCSV_Click(object sender, RoutedEventArgs e)
        {
            if (cBox_dateFormat.SelectedItem == null)
            {
                MessageBox.Show("Please select export dateTime format!");
                return;
            }

            EmployeeHelper.GenerateEmployeesCsvFile(cBox_dateFormat.SelectedValue.ToString(), 30);
        }

        private void FindEmployeePairs(List<Employee> employeesData)
        {
            try
            {
                List<EmployeePair> employeePairs = EmployeeHelper.GetEmployeePairs(employeesData, dateFormat);
                BindDataGrid(dg_ProcessedData, employeePairs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BindDataGrid(DataGrid dataGrid, IEnumerable data)
        {
            dataGrid.AutoGenerateColumns = true;
            dataGrid.ItemsSource = data;

            dataGrid.Columns[0].Width = new DataGridLength(15, DataGridLengthUnitType.Star);
            dataGrid.Columns[1].Width = new DataGridLength(15, DataGridLengthUnitType.Star);
            dataGrid.Columns[2].Width = new DataGridLength(35, DataGridLengthUnitType.Star);
            dataGrid.Columns[3].Width = new DataGridLength(35, DataGridLengthUnitType.Star);
        }

        private void BindDateFormat(List<Employee> employeesData)
        {
            dateFormat = DateHelper.DetermineDateFormat(employeesData.Select(employee => employee.DateFrom).ToList());
            cBox_dateFormat.SelectedItem = dateFormat;
        }
    }
}
