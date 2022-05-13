namespace AutomobileRepairShop.Models
{
    public class AppointClasses
    {
        public static int idToSend;
        public Appointment Appointment { get; set; }
        public Car Car { get; set; }
        public User User { get; set; }
        public double Cost;
        public int billId;

        public AppointClasses() { }

        // for finished appointments
        public AppointClasses(Appointment appointment, Car car, User user, double cost, int BillId)
        {
            Appointment = appointment;
            Car = car;
            User = user;
            Cost = cost;
            billId = BillId;
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
