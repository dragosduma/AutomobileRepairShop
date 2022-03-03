using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Employee
    {
        public int Id { get; set; }
        public double Salary { get; set; }
        public int DepartmentId { get; set; }
        public int IdUser { get; set; }
    }
}
