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

        public virtual ICollection<Bill> Bills { get; set; }
    }
}
