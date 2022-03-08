using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Department
    {
        public Department()
        {
            CarParts = new HashSet<CarPart>();
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<CarPart> CarParts { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
