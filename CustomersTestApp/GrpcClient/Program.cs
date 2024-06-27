using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Core;
using GrpcClient;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set up the channel to the gRPC server
            var channel = GrpcChannel.ForAddress("http://localhost:5203");

            // Create a new client for the CustomerService
            var client = new CustomerService.CustomerServiceClient(channel);

            // Create metadata with the application name
            var headers = new Metadata
            {
                { "app-name", "GrpcClient" }
            };

            // Example of making a CreateCustomer call
            var createResponse = await client.CreateCustomerAsync(
                new CreateCustomerRequest
                {
                    Name = "John Doe",
                    Email = "john@example.com",
                    Discount = 5,
                    CanRemove = true
                }, headers);

            var createdCustomerId1 = createResponse.Customer.Id;
            Console.WriteLine($"Created Customer: {createdCustomerId1}, {createResponse.Customer.Name}");

            var createResponse2 = await client.CreateCustomerAsync(
                new CreateCustomerRequest
                
                    {
                        Name = "Jane Doe",
                        Email = "jane@example.com",
                        Discount = 7,
                        CanRemove = false
                }, headers);

            var createdCustomerId2 = createResponse2.Customer.Id;
            Console.WriteLine($"Created Customer: {createdCustomerId2}, {createResponse2.Customer.Name}");


            // Example of making a GetAllCustomers call
            var getAllResponse = await client.GetAllCustomersAsync(new Google.Protobuf.WellKnownTypes.Empty(), headers);
            foreach (var customer in getAllResponse.Customers)
            {
                Console.WriteLine($"Customer: {customer.Name}, Email: {customer.Email}, Discount: {customer.Discount}");
            }

            // Example of making an UpdateCustomer call
            try
            {
                var updateResponse = await client.UpdateCustomerAsync(
                    new UpdateCustomerRequest
                    {
                        Id = createdCustomerId1,
                        Name = "John Doe Updated",
                        Email = "john_updated@example.com",
                        Discount = 10,
                        CanRemove = true
                    }, headers);

                Console.WriteLine($"Updated Customer: {updateResponse.Customer.Id}, {updateResponse.Customer.Name}");
            }
            catch (RpcException e)
            {
                Console.WriteLine($"Error updating customer: {e.Status.Detail}");
            }

            // Example of making a DeleteCustomer call
            try
            {
                await client.DeleteCustomerAsync(new DeleteCustomerRequest { Id = createdCustomerId1 }, headers);
                Console.WriteLine($"Deleted Customer: {createdCustomerId1}");
            }
            catch (RpcException e)
            {
                Console.WriteLine($"Error deleting customer: {e.Status.Detail}");
            }

            // Example of making a FilterCustomers call
            var filterResponse = await client.FilterCustomersAsync(new FilterCustomersRequest { Name = "Streamed Customer 1", Email = "streamed1@example.com" }, headers);
            foreach (var customer in filterResponse.Customers)
            {
                Console.WriteLine($"Filtered Customer: {customer.Name}, Email: {customer.Email}, Discount: {customer.Discount}");
            }

            // Shut down the channel when done
            await channel.ShutdownAsync();
        }
    }
}
