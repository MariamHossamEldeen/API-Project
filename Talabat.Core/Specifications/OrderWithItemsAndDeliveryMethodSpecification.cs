using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications
{
    public class OrderWithItemsAndDeliveryMethodSpecification : BaseSpecification<Order>
    {
        // This Constructor Is Used For Get All The Orders For A Specific USer
        public OrderWithItemsAndDeliveryMethodSpecification(string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            AddOrederByDescending(O => O.OrderDate);

        }


        // This Constructor Is Used For Get A Specific Order By Id For A Specific USer

        public OrderWithItemsAndDeliveryMethodSpecification(int orderId , string buyerEmail)
            : base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
               
            
        }
    }
}
