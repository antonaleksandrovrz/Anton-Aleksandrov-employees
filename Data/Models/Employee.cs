
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton_Aleksandrov_employees.Data.Models
{
    public class Employee
    {
        public int EmpID { get; set; }
        public int ProjectID { get; set; }
        public string DateFrom { get; set; }
        public string? DateTo { get; set; }
    }
}
