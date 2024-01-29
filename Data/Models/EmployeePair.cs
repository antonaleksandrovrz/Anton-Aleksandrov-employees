using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anton_Aleksandrov_employees.Data.Models
{
    class EmployeePair
    {
        public int EmpID_First { get; set; }
        public int EmpID_Second { get; set; }
        public int ProjectID { get; set; }
        public int DaysWorkedTogether { get; set; }
    }
}
