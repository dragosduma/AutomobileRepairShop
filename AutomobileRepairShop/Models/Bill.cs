using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Bill
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public int CarId { get; set; }
        public int AppointmentId { get; set; }

        public virtual Appointment Appointment { get; set; } = null!;
        public virtual Car Car { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
