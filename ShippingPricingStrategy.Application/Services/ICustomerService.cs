﻿using ShippingPricingStrategy.Domain.Models.Entities;

namespace ShippingPricingStrategy.Application.Services;

public interface ICustomerService
{
    Task<Customer>? GetCustomById(long id);
}