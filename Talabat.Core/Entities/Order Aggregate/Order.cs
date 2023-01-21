using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
                
        }
        public Order(string buyerEmail , Address shipingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal , string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipingAddress = shipingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
          
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShipingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } // Navigation Property [One]
        public ICollection<OrderItem> Items { get; set; } // Navigation Property [Many]
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

         


    }
}
