namespace AutomobileRepairShop.Models
{
    public class Client : User
    {
        
        List<DateTime> appointments = new List<DateTime>();
        List<Car> cars = new List<Car>();
        List<Bill> bills = new List<Bill>();

        public Client(int id, string name, string surname, string adress, int age, string email) : base(id, name, surname, adress, age, email)
        {
            //from base class
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.address = adress;
            this.age = age;
            this.email = email;
        }

        public void addAppointment(DateTime date)   //adds an appointment and sorts the list for better data management
        {
            this.appointments.Add(date);
            this.appointments.Sort();
        }

        //public void checkAppointments(){} //checks if the appointment is overdue or not
        //alternative -> the employee deletes the appointment once the work is over
        //            -> the employee deletes the appointment once the client arrives
        //public void deleteAppointment(){} 

        public void addCar(Car car)   //adds an appointment and sorts the list for better data management
        {
            this.cars.Add(car);
            this.cars.Sort();
            //this.cars = this.cars.OrderBy(o => o.id).ToList();  //alternative sorting by a specific parameter
        }

        public void removeCar(int idToFind)
        {
            this.cars.Remove(cars.Find(cars => cars.id == idToFind));   //lambda to be reviewed
        }

        public void addBill(Bill bill)   //adds an appointment and sorts the list for better data management
        {
            this.bills.Add(bill);
            this.bills.Sort();
            //this.bills = this.bills.OrderBy(o => o.id).ToList();  //alternative sorting by a specific parameter
        }

    }
}
