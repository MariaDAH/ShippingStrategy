using ShippingPricingStrategy.Domain.Models.Dtos;
using ShippingPricingStrategy.Domain.Models.Entities;
using ShippingPricingStrategy.Infraestructure.UnitOfWork;

namespace ShippingPricingStrategy.Application.Services;

public class CheckoutService(ICustomerService customerService,
    IUnitOfWork uow,
    ILogger<CheckoutService> logger): ICheckoutService
{
    private Cart _cart = new();
    private IStrategy _strategy;
    public async Task Scan(Purchase purchase)
    {
        var repoCart =  uow.GetRepository<Cart>();
        var repoService =  uow.GetRepository<Service>();
        var repoPrices =  uow.GetRepository<Price>();
        
        var customer = await customerService.GetCustomById(purchase.CustomerId)!;

        if (customer is null)
        {
            //ToDo: Make custom exception
            throw new Exception("Customer doesn't exit");
        }
        var cart = await repoCart.GetByIdAsync(purchase.CartId)!;
        if (cart is null)
        {
            logger.LogInformation("Creating new cart");
            _cart.Update(0,0);
            _cart.Customer = customer;
             await repoCart.AddAsync(_cart);
        }
        else
        {
            _cart = cart;
            _cart.Customer = customer;
            repoCart.Update(_cart);
        }


        var allPrices = await repoPrices.GetAllAsync();
        var price = allPrices.FirstOrDefault(x => x.ServiceName == purchase.ServiceName);
        if (price is null)
        {
            //ToDo: Make custom exception
            throw new Exception("Price doesn't exit for that service.");
        }
        var service = Service.Create(purchase.ServiceName, purchase.Quantity);
        service.Update(purchase.ServiceName, purchase.Quantity, price, service.TotalAmount, service.TotalDiscount);
        if (await repoService.GetByIdAsync(service.ServiceId) is not null)
        {
            _cart.Services.Find(x => x.ServiceId == service.ServiceId);
            repoService.Update(service);
        }
        else
        {
            var savedService = await repoService.AddAsync(service);
            _cart.Services.Add(savedService);
        }
    }

    public async Task<decimal> GetTotalPrice()
    {
        decimal totalPrice = 0;
        decimal amountDiscount = 0;
        
        var repoCart =  uow.GetRepository<Cart>();
        var repoService =  uow.GetRepository<Service>();
        
        foreach (var service in _cart.Services)
        {
            _strategy = new StrategyIndividual(service);
            var priceIndividual = await _strategy.CalculateTotalPrice() ?? 0;
            logger.LogInformation($"#############################Individual price: {priceIndividual}################################" );

            if (service.Price.MultiPurchasePrice is not null)
            {
                _strategy = new MultiPurchaseStrategy(service);
                var multipurchPrice = await _strategy.CalculateTotalPrice() ?? priceIndividual;
                logger.LogInformation($"#########################Multipurchase price: {multipurchPrice}#########################" );
            
                
                totalPrice = Compare(priceIndividual, multipurchPrice);
                amountDiscount = CalculateAmountDiscounted(priceIndividual, multipurchPrice);
            }
            else
            {
                totalPrice = priceIndividual;
            }

            service.TotalDiscount = amountDiscount;
            service.TotalAmount = totalPrice;
           
            repoService.Update(service);
            _cart.Update(_cart.TotalAmount + totalPrice, _cart.TotalDiscount + amountDiscount);
        }
        
        repoCart.Update(_cart);
        logger.LogInformation($"###########################Cart total price: {_cart.TotalAmount}, total discount: {_cart.TotalDiscount}############" );
        return (decimal)_cart.TotalAmount!;
    }

    private decimal Compare(decimal price1, decimal price2)
    {
        return decimal.Min(price1, price2);
    }

    private decimal CalculateAmountDiscounted(decimal price1, decimal price2)
    {
        return decimal.Subtract(price1, price2);
    }
}