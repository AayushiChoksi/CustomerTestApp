using Grpc.Core;
using CustomerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace CustomerService.Services
{
    public class CustomerServiceImpl : CustomerService.CustomerServiceBase
    {
        private readonly List<Customer> _customers = new List<Customer>
        {
            new Customer { Id = Guid.NewGuid().ToString(), Name = "Alice", Email = "alice@example.com", Discount = 10, CanRemove = true },
            new Customer { Id = Guid.NewGuid().ToString(), Name = "Bob", Email = "bob@example.com", Discount = 15, CanRemove = false }
        };

        public override Task<CustomerResponse> GetCustomer(CustomerRequest request, ServerCallContext context)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == request.Id);
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            return Task.FromResult(new CustomerResponse { Customer = customer });
        }

        public override Task<CustomersResponse> GetAllCustomers(Empty request, ServerCallContext context)
        {
            var response = new CustomersResponse();
            response.Customers.AddRange(_customers);
            return Task.FromResult(response);
        }

        public override Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            var newCustomer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                Discount = request.Discount,
                CanRemove = request.CanRemove
            };

            _customers.Add(newCustomer);

            return Task.FromResult(new CustomerResponse { Customer = newCustomer });
        }

        public override Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == request.Id);
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Discount = request.Discount;
            customer.CanRemove = request.CanRemove;

            return Task.FromResult(new CustomerResponse { Customer = customer });
        }

        public override Task<Empty> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == request.Id);
            if (customer == null || !customer.CanRemove)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found or cannot be removed"));
            }

            _customers.Remove(customer);

            return Task.FromResult(new Empty());
        }
    }
}
