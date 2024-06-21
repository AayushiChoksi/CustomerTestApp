namespace CustomerService.Services;

public class CustomerService : IPC.CustomerService.CustomerServiceBase
{
    private readonly ILogger<CustomerService> _logger;
    public CustomerService(ILogger<CustomerService> logger)
    {
        _logger = logger;
    }
}
