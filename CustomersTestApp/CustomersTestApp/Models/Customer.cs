namespace CustomersTestApp.Models
{
    public class Customer
    {
        public Customer()
        {
            Id = Guid.NewGuid().ToString(); // Generates a unique ID
            Can_Remove = true;
        }

        public Customer(string id, string name, string email, int discount, bool canRemove)
        {
            Id = id;
            Name = name;
            Email = email;
            Discount = discount;
            Can_Remove = canRemove;
        }

        public string Id { get; private set; } // Ensure Id is of type string
        public string Name { get; set; }
        public string Email { get; set; }
        public int Discount { get; set; } // min = 0, max = 30
        public bool Can_Remove { get; set; }
    }
}
