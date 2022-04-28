namespace AutomobileRepairShop.Models
{
    public class AppointClasses
    {
        public Appointment Appointment { get; set; }
        public Car Car { get; set; }
        public User User { get; set; }

        public double Cost;

        public AppointClasses() { }

        // for finished appointments
        public AppointClasses(Appointment appointment, Car car, User user, double cost)
        {
            Appointment = appointment;
            Car = car;
            User = user;
            Cost = cost;
        }

        // for unfinished app.
        public AppointClasses(Appointment appointment, Car car, User user)
        {
            Appointment = appointment;
            Car = car;
            User = user;
        }
    }
}
