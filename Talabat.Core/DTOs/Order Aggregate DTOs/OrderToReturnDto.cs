using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Core.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; } // To receive it from user as a string

        public Address ShippingAddress { get; set; }

        public ICollection<OrderItemDto> Items { get; set; }

        public string DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; } // Get value from GetTotal method in Order class

        public string PaymentIntentId { get; set; }
    }
}
