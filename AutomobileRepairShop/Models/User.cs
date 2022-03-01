namespace AutomobileRepairShop.Models
{
    public class User
    {
        protected int id { get { return id; } set { id = value; } }
        protected string name { get { return name; } set { name = value; } }
        protected string surname { get { return surname; } set { surname = value; } }
        protected string address { get { return address; } set { address = value; } }
        protected int age { get { return age; } set { age = value; } }
        protected string email { get { return email; } set { email = value; } }
        protected string password 
        { 
            private get { return password; }    //private getter for better security pls review
            set { password = value; } 
        }    

        public User(int id, string name, string surname, string adress, int age, string email)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.address = adress;
            this.age = age;
            this.email = email;                
        }

        public override string ToString()
        {
            //to be rewritten; final needed form unknown
            return this.name+" "+this.surname+" "+this.email;
        }
    }
}
