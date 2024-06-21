namespace CustomersTestApp;

public class Customer
{
    public Customer()
    {
        Can_Remove = true;
    }

    public Guid Id { get; set; } // auto generated with random, without repeating, read-only
    public string name { get; set; }
    public string Email { get; set; }
    public int Discount { get; set; } // min = 0, max = 30
    public bool Can_Remove { get; set; }
}