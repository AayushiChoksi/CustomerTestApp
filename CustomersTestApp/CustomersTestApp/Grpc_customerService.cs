using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CustomersTestApp.Models;

namespace CustomersTestApp
{
    public class Grpc_customerService : ICustomerService
    {
        private List<Customer> _customers;

        #region Constructors

        public Grpc_customerService()
        {
            _customers = new List<Customer>
            {
                new Customer
                {
                    Name = "Microsoft",
                    Email = "microsoft@microsoft.com",
                    Discount = 10,
                    Can_Remove = false
                },
                new Customer
                {
                    Name = "Google",
                    Email = "google@google.com",
                    Discount = 5,
                    Can_Remove = false
                },
                new Customer
                {
                    Name = "Amazon",
                    Email = "amazon@amazon.com",
                    Discount = 0,
                    Can_Remove = true
                }
            };
        }

        #endregion Constructors

        public Customer[] GetCustomersArray()
        {
            return _customers.ToArray();
        }

        public Task<bool> AddCustomer(Customer c)
        {
            _customers.Add(c);
            return Task.FromResult(true);
        }

        public IAsyncEnumerable<Customer> GetCustomers(CancellationToken cancellationToken)
        {
            return _customers.ToAsyncEnumerable();
        }

        public Task<bool> UpdateCustomer(Customer c)
        {
            var existingCustomer = _customers.FirstOrDefault(cust => cust.Id == c.Id);
            if (existingCustomer != null)
            {
                existingCustomer.Name = c.Name;
                existingCustomer.Email = c.Email;
                existingCustomer.Discount = c.Discount;
                existingCustomer.Can_Remove = c.Can_Remove;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RemoveCustomer(string id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer != null && customer.Can_Remove)
            {
                _customers.Remove(customer);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        // No database is needed
        // Assume we have a perfect service that is always online.
    }
}
