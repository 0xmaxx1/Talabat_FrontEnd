using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Models.Basket;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Models.Product;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IGenericRepository<Product> _productsRepository;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodsRepository;
        //private readonly IGenericRepository<Order> _ordersRepository;

        public OrderService(IBasketRepository basketRepository,
            IPaymentService paymentService,
            //IGenericRepository<Product> productsRepository,
            //IGenericRepository<DeliveryMethod> deliveryMethodsRepository,
            //IGenericRepository<Order> ordersRepository
            IUnitOfWork unitOfWork
            )
        {
            _basketRepository = basketRepository;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            //_productsRepository = productsRepository;
            //_deliveryMethodsRepository = deliveryMethodsRepository;
            //_ordersRepository = ordersRepository;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1. Get Basket form Baskets Repo
            var basket = await _basketRepository.GetBasketAsync(basketId);


            // 2. Get Selected Items at Basket From Products Repo [catch every item in the basket and create order item]
            List<OrderItem> orderItems = new List<OrderItem>(); // Container for OrderItems

            if (basket?.Items?.Count > 0) // Check if basket has items or not
            {
                foreach (BasketItem item in basket.Items) // Loop through each item in the basket
                { // in basket item i 'll trust only quantity and product id
                    Product product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id); // Get Product from Products Repo

                    // Create ProductItemOrdered object for OrderItem
                    ProductItemOrdered productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureURL);

                    // Create OrderItem for each product in the basket
                    OrderItem orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    // Add created orderItem to orderItems List
                    orderItems.Add(orderItem);
                }
            }


            // 3. Calculate SubTotal
            decimal subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method from Delivery Methods Repo
            DeliveryMethod deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);


            // Check if there is order created with the same paymentIntentId and delete it
            var orderSpec = new OrderWithPaymentIntentIdSpecifications(basket.PaymentIntentId);

            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(orderSpec);

            if (existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);

                // Update PaymentIntentId in the basket if it removed when existing item is deleted
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            // 5. Create Order
            Order order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);

            // 6. Add Order to Orders Repo
            await _unitOfWork.Repository<Order>().AddAsync(order);

            // 7. Save in Database 
            int result = await _unitOfWork.Complete();
            if (result <= 0)
                return null;
            else
                return order;
        }



        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            OrdersForUserSpecifications? orderSpecs = new OrdersForUserSpecifications(buyerEmail);

            IReadOnlyList<Order> orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(orderSpecs);

            return orders;
        }


        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            OrdersForUserSpecifications orderSpec = new OrdersForUserSpecifications(orderId, buyerEmail);

            Order order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(orderSpec);

            return order;
        }


        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            IReadOnlyList<DeliveryMethod> deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            if (deliveryMethods is null)
                return null;
            else
                return deliveryMethods;
        }
    }
}
