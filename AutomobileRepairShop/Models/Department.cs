using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Department
    {
        public Department()
        {
            CarParts = new HashSet<CarPart>();
            Users = new HashSet<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<CarPart> CarParts { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
