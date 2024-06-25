using CustomerService.Models;

namespace CustomerService.Repositories
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(string customerId);
        List<Customer> FilterCustomers(string filter, string filterType);
    }
}
