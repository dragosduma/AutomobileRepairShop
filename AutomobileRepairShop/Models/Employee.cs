namespace AutomobileRepairShop.Models
{
    public class Employee : User
    {
        private double salary { get { return salary; } set { salary = value; } }
        private int departmentId { get { return departmentId; } set { departmentId = value; } }


        public Employee(int id, string name, string surname, string adress, int age, string email, double salary, int departmentId) : base(id, name, surname, adress, age, email)
        {
            //from base class
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.address = adress;
            this.age = age;
            this.email = email;

            //from the derived class
            this.salary = salary;
            this.departmentId = departmentId;
        }
    }
}
