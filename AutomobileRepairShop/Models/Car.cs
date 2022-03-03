using System;
using System.Collections.Generic;

namespace AutomobileRepairShop.Models
{
    public partial class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string ChassisCode { get; set; } = null!;
        public int Kilometers { get; set; }
        public int IdClient { get; set; }
    }
}
