namespace AutomobileRepairShop.Models
{
    public class CarParts
    {
        public int id;
        public int departmentId;
        public string name;
        public double price;
        public string laborName;
        public double laborPrice;

        public CarParts(int id, int departmentId, string name, double price, string laborName, double laborPrice)
        {
            this.id = id;
            this.departmentId = departmentId;
            this.name = name;
            this.price = price;
            this.laborName = laborName;
            this.laborPrice = laborPrice;
        }
    }
}
