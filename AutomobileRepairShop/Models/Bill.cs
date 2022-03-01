namespace AutomobileRepairShop.Models
{
    public class Bill
    {
        public int id { get { return id; } set { id = value; } }
        public int idClient { get { return idClient; } set { idClient = value; } }
        public int idCar { get { return idCar; } set { idCar = value; } }
        public string description { get { return description; } set { description = value; } }
        public double price { get { return price; } set { price = value; } }

        public Bill(int id, int idClient, int idCar, string description, double price)
        {
            this.id = id;
            this.idClient = idClient;
            this.idCar = idCar;
            this.description = description;
            this.price = price;
        }
    }
}
