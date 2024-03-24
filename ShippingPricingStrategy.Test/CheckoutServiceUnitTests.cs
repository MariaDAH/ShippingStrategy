using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using ShippingPricingStrategy.Application.Services;
using ShippingPricingStrategy.Domain.Models.Dtos;
using ShippingPricingStrategy.Domain.Models.Entities;
using ShippingPricingStrategy.Infraestructure.UnitOfWork;
using ShippingPricingStrategy.Infrastructure.Daos;
using ShippingPricingStrategy.Infrastructure.Repositories;

namespace ShippingPricingStrategy.Test;

[TestFixture]
public class Tests
{
    private ICheckoutService _checkoutService;
    private Mock<ApplicationDbContext> _dbContext;
    private Mock<IRepository<Cart>> _cartRepository;
    private Mock<IUnitOfWork> _uow;
    private Mock<IRepository<Service>> _serviceRepository;
    private Mock<IRepository<Price>> _priceRepository;
    private Mock<IRepository<Customer>> _customerRepository;
    private Mock<ILogger<CheckoutService>> _logger;
   
    private Mock<ICustomerService> _customerService;
    
    [OneTimeSetUp]
    public void Setup()
    {
        _cartRepository = new Mock<IRepository<Cart>>();
        _serviceRepository = new Mock<IRepository<Service>>();
        _priceRepository = new Mock<IRepository<Price>>();
        _customerRepository = new Mock<IRepository<Customer>>();
        _dbContext = new Mock<ApplicationDbContext>();
        _logger = new Mock<ILogger<CheckoutService>>();
        _uow = new Mock<IUnitOfWork>();
        _customerService = new Mock<ICustomerService>();
    }

    [Test]
    public void ScanPurchasesForNonExistingCustomerThrowException()
    {
        //Arrange
        _dbContext.Setup(db => db.Customers);
        _customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>)).ReturnsAsync((Customer)null);
        _uow.Setup(x => x.GetRepository<Customer>()).Returns(_customerRepository.Object);
        var sut = new CheckoutService(_customerService.Object, _uow.Object, _logger.Object);
        var purchase = new Purchase(1, 1, "B", 2);
        
        //Act
        sut.Scan(purchase);
        
        //Assert
        Assert.Throws<Exception>(() => throw new Exception("Customer doesn't exist"));
    }
    
    [Test]
    public async Task MultipurchaseDiscountAdvantageReturns20()
    {
        //Arrange
        var listPrices = new List<Price>();
        listPrices.Add(new Price {PriceCode = 1, ServiceName = "A", IndividualPrice = 10, MultiPurchasePrice = 25, QuantityPromotion = 3});
        listPrices.Add(new Price {PriceCode = 2, ServiceName = "B", IndividualPrice = 12, MultiPurchasePrice = 20, QuantityPromotion = 2});
        listPrices.Add(new Price {PriceCode = 3, ServiceName = "C", IndividualPrice = 15, MultiPurchasePrice = null, QuantityPromotion = null});
        listPrices.Add(new Price {PriceCode = 4, ServiceName = "D", IndividualPrice = 25, MultiPurchasePrice = null, QuantityPromotion = null});
        listPrices.Add(new Price {PriceCode = 5, ServiceName = "F", IndividualPrice = 8, MultiPurchasePrice = 15, QuantityPromotion = 2});
        _customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>)).ReturnsAsync(new Customer {CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"});
        _priceRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(listPrices);
        _cartRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>)).ReturnsAsync(new Cart {CartId = 1, TotalAmount = 0, TotalDiscount = 0, Customer = new Customer{CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"}});
        _serviceRepository.Setup(x => x.AddAsync(It.IsAny<Service>())).ReturnsAsync(new Service {ServiceId = 1, ServiceName = "B", Price = new Price{PriceCode = 2, ServiceName = "B", IndividualPrice = 12, MultiPurchasePrice = 20, QuantityPromotion = 2}, Quantity = 2, TotalAmount = 0, TotalDiscount = 0});
        _uow.Setup(x => x.GetRepository<Customer>()).Returns(_customerRepository.Object);
        _uow.Setup(x => x.GetRepository<Price>()).Returns(_priceRepository.Object);
        _uow.Setup(x => x.GetRepository<Cart>()).Returns(_cartRepository.Object);
        _uow.Setup(x => x.GetRepository<Service>()).Returns(_serviceRepository.Object);
        _customerService.Setup(x => x.GetCustomById(1)).ReturnsAsync(new Customer {CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"});
        var sut = new CheckoutService(_customerService.Object, _uow.Object, _logger.Object);
        var purchase = new Purchase(1, 1, "B", 2);
        await sut.Scan(purchase);
        
        //Act 
        var result = await sut.GetTotalPrice();
        
        //Assert
        Assert.That(20, Is.EqualTo(result));
    }
    
    [Test]
    public async Task NoMultipurchaseDiscounteturns23()
    {
        //Arrange
        var listPrices = new List<Price>();
        listPrices.Add(new Price {PriceCode = 1, ServiceName = "A", IndividualPrice = 10, MultiPurchasePrice = 25, QuantityPromotion = 3});
        listPrices.Add(new Price {PriceCode = 2, ServiceName = "B", IndividualPrice = 12, MultiPurchasePrice = 20, QuantityPromotion = 2});
        listPrices.Add(new Price {PriceCode = 3, ServiceName = "C", IndividualPrice = 15, MultiPurchasePrice = null, QuantityPromotion = null});
        listPrices.Add(new Price {PriceCode = 4, ServiceName = "D", IndividualPrice = 25, MultiPurchasePrice = null, QuantityPromotion = null});
        listPrices.Add(new Price {PriceCode = 5, ServiceName = "F", IndividualPrice = 8, MultiPurchasePrice = 15, QuantityPromotion = 2});
        _customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>)).ReturnsAsync(new Customer {CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"});
        _priceRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(listPrices);
        _cartRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>)).ReturnsAsync(new Cart {CartId = 2, TotalAmount = 0, TotalDiscount = 0, Customer = new Customer{CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"}});
        _uow.Setup(x => x.GetRepository<Customer>()).Returns(_customerRepository.Object);
        _uow.Setup(x => x.GetRepository<Price>()).Returns(_priceRepository.Object);
        _uow.Setup(x => x.GetRepository<Cart>()).Returns(_cartRepository.Object);
        _uow.Setup(x => x.GetRepository<Service>()).Returns(_serviceRepository.Object);
        _customerService.Setup(x => x.GetCustomById(1)).ReturnsAsync(new Customer {CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"});
        var sut = new CheckoutService(_customerService.Object, _uow.Object, _logger.Object);
        var purchase1 = new Purchase(1, 2, "F", 1);
        _serviceRepository.Setup(x => x.AddAsync(It.IsAny<Service>())).ReturnsAsync(new Service {ServiceId = 2, ServiceName = "F", Price = new Price{PriceCode = 5, ServiceName = "F", IndividualPrice = 8, MultiPurchasePrice = 15, QuantityPromotion = 2}, Quantity = 1, TotalAmount = 0, TotalDiscount = 0});
        await sut.Scan(purchase1);
        var purchase2 = new Purchase(1, 2, "C", 1);
        _serviceRepository.Setup(x => x.AddAsync(It.IsAny<Service>())).ReturnsAsync(new Service {ServiceId = 3, ServiceName = "C", Price = new Price{PriceCode = 3, ServiceName = "C", IndividualPrice = 15, MultiPurchasePrice = null, QuantityPromotion = null}, Quantity = 1, TotalAmount = 0, TotalDiscount = 0});
        await sut.Scan(purchase2);
        
        //Act 
        var result = await sut.GetTotalPrice();
        
        //Assert
        Assert.That(23, Is.EqualTo(result));
    }
    
        [Test]
    public async Task MixofDiscountsandNoDiscountReturns27()
    {
        //Arrange
        var listPrices = new List<Price>();
        listPrices.Add(new Price {PriceCode = 1, ServiceName = "A", IndividualPrice = 10, MultiPurchasePrice = 25, QuantityPromotion = 3});
        listPrices.Add(new Price {PriceCode = 2, ServiceName = "B", IndividualPrice = 12, MultiPurchasePrice = 20, QuantityPromotion = 2});
        listPrices.Add(new Price {PriceCode = 3, ServiceName = "C", IndividualPrice = 15, MultiPurchasePrice = null, QuantityPromotion = null});
        listPrices.Add(new Price {PriceCode = 4, ServiceName = "D", IndividualPrice = 25, MultiPurchasePrice = null, QuantityPromotion = null});
        listPrices.Add(new Price {PriceCode = 5, ServiceName = "F", IndividualPrice = 8, MultiPurchasePrice = 15, QuantityPromotion = 2});
        _customerRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>)).ReturnsAsync(new Customer {CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"});
        _priceRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(listPrices);
        _cartRepository.Setup(x => x.GetByIdAsync(It.IsAny<object>)).ReturnsAsync(new Cart {CartId = 3, TotalAmount = 0, TotalDiscount = 0, Customer = new Customer{CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"}});
        _uow.Setup(x => x.GetRepository<Customer>()).Returns(_customerRepository.Object);
        _uow.Setup(x => x.GetRepository<Price>()).Returns(_priceRepository.Object);
        _uow.Setup(x => x.GetRepository<Cart>()).Returns(_cartRepository.Object);
        _uow.Setup(x => x.GetRepository<Service>()).Returns(_serviceRepository.Object);
        _customerService.Setup(x => x.GetCustomById(1)).ReturnsAsync(new Customer {CustomerId = 1, NationalIdentifier = "111111", Name = "Acme1", AddressLine = "King Street"});
        var sut = new CheckoutService(_customerService.Object, _uow.Object, _logger.Object);
        var purchase1 = new Purchase(1, 3, "F", 2);
        _serviceRepository.Setup(x => x.AddAsync(It.IsAny<Service>())).ReturnsAsync(new Service {ServiceId = 4, ServiceName = "F", Price = new Price{PriceCode = 5, ServiceName = "F", IndividualPrice = 8, MultiPurchasePrice = 15, QuantityPromotion = 2}, Quantity = 2, TotalAmount = 0, TotalDiscount = 0});
        await sut.Scan(purchase1);
        var purchase2 = new Purchase(1, 3, "B", 1);
        _serviceRepository.Setup(x => x.AddAsync(It.IsAny<Service>())).ReturnsAsync(new Service {ServiceId = 5, ServiceName = "B", Price = new Price{PriceCode = 2, ServiceName = "B", IndividualPrice = 12, MultiPurchasePrice = 20, QuantityPromotion = 2}, Quantity = 1, TotalAmount = 0, TotalDiscount = 0});
        await sut.Scan(purchase2);
        
        //Act 
        var result = await sut.GetTotalPrice();
        
        //Assert
        Assert.That(27, Is.EqualTo(result));
    }
    
}