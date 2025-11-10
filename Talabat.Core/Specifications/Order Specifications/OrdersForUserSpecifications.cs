using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specifications
{
    public class OrdersForUserSpecifications : BaseSpecification<Order>
    {
        // Ctor for All User Orders
        public OrdersForUserSpecifications(string email)
            : base(O => O.BuyerEmail == email) // Where Condition [Criteria]
        {
            // Includes
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);

            // Order By
            AddOrderByDesc(O => O.OrderDate);
        }

        // Ctor for Specific Order by Id
        public OrdersForUserSpecifications(int orderId, string buyerEmail)
            : base(O => O.Id == orderId && O.BuyerEmail == buyerEmail) // Where Conditions
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
