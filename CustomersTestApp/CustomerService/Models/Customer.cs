namespace CustomerService.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Discount { get; set; }
        public bool CanRemove { get; set; }
    }
}
