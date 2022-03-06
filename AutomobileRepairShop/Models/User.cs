using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdRole { get; set; }

        public virtual Role IdRoleNavigation { get; set; } = null!;
    }
}
