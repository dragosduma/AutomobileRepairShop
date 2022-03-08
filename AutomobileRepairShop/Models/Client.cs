using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Client
    {
        public Client()
        {
            Bills = new HashSet<Bill>();
        }

        public int Id { get; set; }

        public virtual User IdNavigation { get; set; } = null!;
        public virtual ICollection<Bill> Bills { get; set; }
    }
}
