namespace AutomobileRepairShop.Models
{
    public class AppointClasses
    {
        public Appointment Appointment { get; set; }
        public Car Car { get; set; }
        public User User { get; set; }

        public AppointClasses() { }
        public AppointClasses(Appointment appointment, Car car, User user)
        {
            Appointment = appointment;
            Car = car;
            User = user;
        }
    }
}
