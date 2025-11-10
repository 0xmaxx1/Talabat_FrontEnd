using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specifications;
using Product = Talabat.Core.Models.Product.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];


            CustomerBasket? basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket is null)
                return null;


            // Check on Delivery Method
            decimal shippingPrice = 0;
            if (basket.DeliveryMethodId.HasValue)
            {
                DeliveryMethod deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                shippingPrice = deliveryMethod.Cost;

                basket.ShippingCost = deliveryMethod.Cost;
            }

            // Check on Basket Items and Items Price
            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    Product? product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }


            PaymentIntentService? service = new PaymentIntentService();

            // Create Or Update Payment Intent
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create PaymentIntent
            {
                PaymentIntentCreateOptions options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(basket.Items.Sum(item => item.Price * item.Quantity) + shippingPrice) * 100, // *100 --> as Stripe works with cents
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await service.CreateAsync(options);

                // Update Basket with PaymentIntentId and ClientSecret
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update PaymentIntent
            {
                PaymentIntentUpdateOptions? options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(basket.Items.Sum(item => item.Price * item.Quantity + shippingPrice) * 100)
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }


            // Update Basket
            await _basketRepository.UpdateBasketAsync(basket);

            return basket;
        }


        public async Task<Order> UpdatePaymentIntentIdSucceededOrFailed(string paymentIntentId, bool isSucceeded)
        {
            OrderWithPaymentIntentIdSpecifications orderSpec = new OrderWithPaymentIntentIdSpecifications(paymentIntentId);

            Order order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(orderSpec);

            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;
        }
    }
}
