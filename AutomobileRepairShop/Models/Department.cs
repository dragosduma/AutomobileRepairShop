namespace AutomobileRepairShop.Models
{
    public class Department
    {
        private int id;
        private string name;

        private List<Employee> employees = new List<Employee>();

        private List<CarParts> carParts = new List<CarParts>();

        public Department(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public void addEmployee(Employee emp)
        {
            employees.Add(emp);
        }

        public void addCarPart(CarParts carPart)
        {
            carParts.Add(carPart);
            carParts.Sort();
            //carParts = carParts.OrderBy(o => o.id).ToList();  //alternative sorting by a specific parameter
        }

    }
}
