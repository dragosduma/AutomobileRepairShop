using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Car
    {
        public Car()
        {
            Appointments = new HashSet<Appointment>();
            Bills = new HashSet<Bill>();
        }
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string ChassisCode { get; set; } = null!;
        public int Kilometers { get; set; }
        public int IdUser { get; set; }
        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
