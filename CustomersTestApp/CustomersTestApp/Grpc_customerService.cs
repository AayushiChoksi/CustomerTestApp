namespace CustomersTestApp;

public class Grpc_customerService : ICustomerService
{
    public Customer[] get_cust()
    {
        return _List1;
    }

    #region Constructors

    public Grpc_customerService()
    {
    }

    #endregion Constructors

    private Customer[] _List1 =
    [
        new Customer
        {
            Id = Guid.NewGuid(),
            name = "Microsoft",
            Email = "microsoft@microsoft.com",
            Discount = 10,
            Can_Remove = false
        },
        new Customer
        {
            Id = Guid.NewGuid(),
            name = "Google",
            Email = "google@google.com",
            Discount = 5,
            Can_Remove = false
        },
        new Customer
        {
            Id = Guid.NewGuid(),
            name = "Amazon",
            Email = "amazon@amazon.com",
            Discount = 0,
            Can_Remove = true
        }
    ];

    public Task<bool> AddCustomer(Customer c)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Customer> GetCustomers(CancellationToken cancellationToken)
    {
        return _List1.ToAsyncEnumerable();
    }

    // todo implement an asynchronous CRUD
    // No database is needed
    // Assume we have a perfect service that is always online.
}