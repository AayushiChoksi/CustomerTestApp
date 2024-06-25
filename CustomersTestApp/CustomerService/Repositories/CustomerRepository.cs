using CustomerService.Models;
using System.Collections.Generic;
using System.Linq;

namespace CustomerService.Repositories
{
    public class CustomerRepository
    {
        private readonly List<Customer> _customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid().ToString(), Name = "Alice", Email = "alice@example.com", Discount = 10, CanRemove = true },
            new Customer { Id = Guid.NewGuid().ToString(), Name = "Bob", Email = "bob@example.com", Discount = 15, CanRemove = false }
        };

        public List<Customer> GetCustomers()
        {
            return _customers;
        }

        public Customer GetCustomerById(string id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public void AddCustomer(Customer customer)
        {
            _customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            var existingCustomer = _customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomer != null)
            {
                existingCustomer.Name = customer.Name;
                existingCustomer.Email = customer.Email;
                existingCustomer.Discount = customer.Discount;
                existingCustomer.CanRemove = customer.CanRemove;
            }
        }

        public void DeleteCustomer(string id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer != null && customer.CanRemove)
            {
                _customers.Remove(customer);
            }
        }
    }
}
