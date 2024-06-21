namespace CustomerService;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddGrpc();

        WebApplication app = builder.Build();
        app.MapGrpcService<Services.CustomerService>();
        app.Run();
    }
}