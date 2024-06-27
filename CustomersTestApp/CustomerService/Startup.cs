using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using CustomerService.Data;
using CustomerService.Repositories;
using CustomerService.Services;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<LoggingInterceptor>();
        });

        // Add gRPC reflection for development purposes
        services.AddGrpcReflection();

        // Configure DbContext with SQLite (change to UseSqlServer if you use SQL Server)
        services.AddDbContext<CustomerContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

        // Register repository and service implementations
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<CustomerServiceImpl>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<CustomerServiceImpl>();

            if (env.IsDevelopment())
            {
                endpoints.MapGrpcReflectionService();
            }

            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client.");
            });
        });
    }
}
