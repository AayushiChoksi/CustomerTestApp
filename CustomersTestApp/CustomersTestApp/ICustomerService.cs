namespace CustomersTestApp;

public interface ICustomerService
{
    //Implementig crud operation

    //

    IAsyncEnumerable<Customer> GetCustomers(CancellationToken cancellationToken);
}
