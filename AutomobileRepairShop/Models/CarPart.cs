using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class CarPart
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; } = null!;
        public double LaborPrice { get; set; }
        public string LaborName { get; set; } = null!;

        public virtual Department Department { get; set; } = null!;
    }
}
