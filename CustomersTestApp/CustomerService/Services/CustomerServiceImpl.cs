using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using CustomerService;
using CustomerService.Data;
using CustomerService.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Services
{
    public class CustomerServiceImpl : CustomerService.CustomerServiceBase
    {
        private readonly CustomerContext _context;

        public CustomerServiceImpl(CustomerContext context)
        {
            _context = context;
        }

        public override async Task<CustomerResponse> GetCustomer(CustomerRequest request, ServerCallContext context)
        {
            var customer = await _context.Customers.FindAsync(request.Id);
            if (customer == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            return new CustomerResponse
            {
                Customer = new Customer
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Discount = customer.Discount,
                    CanRemove = customer.CanRemove
                }
            };
        }

        public override async Task<CustomersResponse> GetAllCustomers(Empty request, ServerCallContext context)
        {
            var customers = await _context.Customers.ToListAsync();
            var response = new CustomersResponse();
            response.Customers.AddRange(customers.Select(customer => new Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Discount = customer.Discount,
                CanRemove = customer.CanRemove
            }));
            return response;
        }

        public override async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            var newCustomer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                Discount = request.Discount,
                CanRemove = request.CanRemove
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Customer added: {newCustomer.Id}");

            return new CustomerResponse
            {
                Customer = new Customer
                {
                    Id = newCustomer.Id,
                    Name = newCustomer.Name,
                    Email = newCustomer.Email,
                    Discount = newCustomer.Discount,
                    CanRemove = newCustomer.CanRemove
                }
            };
        }

        public override async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        {
            var customer = await _context.Customers.FindAsync(request.Id);
            if (customer == null)
            {
                Console.WriteLine($"Customer not found: {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found"));
            }

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.Discount = request.Discount;
            customer.CanRemove = request.CanRemove;

            await _context.SaveChangesAsync();

            return new CustomerResponse
            {
                Customer = new Customer
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Discount = customer.Discount,
                    CanRemove = customer.CanRemove
                }
            };
        }

        public override async Task<Empty> DeleteCustomer(DeleteCustomerRequest request, ServerCallContext context)
        {
            var customer = await _context.Customers.FindAsync(request.Id);
            if (customer == null || !customer.CanRemove)
            {
                Console.WriteLine($"Customer not found or cannot be removed: {request.Id}");
                throw new RpcException(new Status(StatusCode.NotFound, "Customer not found or cannot be removed"));
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            Console.WriteLine($"Customer removed: {request.Id}");

            return new Empty();
        }

        public override async Task<CustomersResponse> FilterCustomers(FilterCustomersRequest request, ServerCallContext context)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(c => c.Name.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(c => c.Email.Contains(request.Email));
            }

            var customers = await query.ToListAsync();

            var response = new CustomersResponse();
            response.Customers.AddRange(customers.Select(customer => new Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Discount = customer.Discount,
                CanRemove = customer.CanRemove
            }));

            return response;
        }
    }
}
