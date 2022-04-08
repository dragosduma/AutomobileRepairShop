using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int IdUser { get; set; }

        public virtual User IdUserNavigation { get; set; } = null!;
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
