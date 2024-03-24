using Microsoft.EntityFrameworkCore;
using ShippingPricingStrategy.Application;
using ShippingPricingStrategy.Application.Services;
using ShippingPricingStrategy.Infraestructure.UnitOfWork;
using ShippingPricingStrategy.Infrastructure.Daos;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["Configuration:ConnectionString"]);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStrategy, Strategy>(sp => sp.GetRequiredService<Strategy>());
builder.Services.AddScoped<ICustomerService,CustomerService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddHostedService<FileProcessingWorker>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<FileProcessingWorker>>();
    var path = Path.Combine(builder.Environment.ContentRootPath, "Purchases");
    var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
    return new FileProcessingWorker(logger, @path, scopeFactory);
});

var host = builder.Build();
host.Run();