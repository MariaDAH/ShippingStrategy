using System.Text.Json;
using ShippingPricingStrategy.Application.Services;
using ShippingPricingStrategy.Domain.Models.Dtos;

namespace ShippingPricingStrategy.Application;

public class FileProcessingWorker(ILogger<FileProcessingWorker> logger, string directoryToWatch, IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var purchases = new List<Purchase>();
        using var fileSystemWatcher = new FileSystemWatcher(directoryToWatch);
        fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
        fileSystemWatcher.Created += async (sender, e) =>
        {
            if (!File.Exists(e.FullPath))
                return;

            Console.WriteLine($"New file detected: {e.FullPath}");
            
             await ProcessFileAsync(e.FullPath);
        };

        fileSystemWatcher.EnableRaisingEvents = true;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
                
            await Task.Delay(1000, stoppingToken);
        }
    }
    
    private async Task ProcessFileAsync(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        var purchases = JsonSerializer.Deserialize<List<Purchase>>(jsonString);

        if (purchases == null) throw new Exception("No purchases in file");

        using var scope = scopeFactory.CreateScope();
        var checkoutService = scope.ServiceProvider.GetRequiredService<ICheckoutService>();
        foreach (var purchase in purchases)
        {
            await checkoutService.Scan(purchase);
        }

        checkoutService.GetTotalPrice();
    }
}