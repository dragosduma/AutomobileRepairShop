namespace AutomobileRepairShop.Models
{
    public class Car
    {
        public int id { get { return id; } private set { id = value; } }
        private string model { get { return model; } set { model = value; } }
        private string brand { get { return brand; } set { brand = value; } }
        private string chassisCode { get { return chassisCode; } set { chassisCode = value; } }
        private int kilometers { get { return kilometers; } set { kilometers = value; } }
        private int idClient { get { return idClient; } set { } }

        public Car(int id, string model, string brand, string chassisCode, int kilometers, int idClient)
        {
            this.id = id;
            this.model = model;
            this.brand = brand;
            this.chassisCode = chassisCode;
            this.kilometers = kilometers;
            this.idClient = idClient;
        }
    }
}
