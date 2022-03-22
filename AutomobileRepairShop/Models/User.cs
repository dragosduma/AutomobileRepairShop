using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomobileRepairShop.Models
{
    public partial class User
    {
        public User()
        {
            Bills = new HashSet<Bill>();
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Address { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdRole { get; set; }
        public int? IdDepartment { get; set; }

        public virtual Department? IdDepartmentNavigation { get; set; }
        public virtual Role IdRoleNavigation { get; set; } = null!;
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Car> Cars { get; set; }


        public List<User> userList = new List<User>();
      
    }
}
