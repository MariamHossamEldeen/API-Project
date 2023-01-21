using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _ordersRepo;

        public OrderService(IBasketRepository basketRepo ,
                            //IGenericRepository<Product> productsRepo,
                            //IGenericRepository<DeliveryMethod> deliveryMethodRepo,
                            //IGenericRepository<Order> ordersRepo
                            IUnitOfWork unitOfWork ,
                            IPaymentService paymentService
            
                                )
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;

            //_productsRepo = productsRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_ordersRepo = ordersRepo;
        }

        

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1 - get basket from baskets repo

            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2 - get selected items at basket from products repo 

            var orderItems = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(product.Id , product.Name , product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price , item.Quantity);
                orderItems.Add(orderItem);
            }

            // 3 - calculate subtotal 

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4 - get delivery method from delivery methods repo 

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5 - create order

            var spec = new OrderWithPaymentIntentIdSpecification(basket.PaymentIntentId);
            var exisitingOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (exisitingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(exisitingOrder);
                await _paymentService.CreateOrUpdatePaymentIntentAsync(basket.Id);
            }

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems , subTotal , basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().CreateAsync(order);

            // 6 - save to database [TODO]

            var result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveryMethods;

        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecification(orderId , buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndDeliveryMethodSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
         
    }
}
