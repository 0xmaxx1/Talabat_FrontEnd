using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class OrderItem : BaseEntity
    {
        //public int ProductId { get; set; }

        //public string ProductName { get; set; }

        //public string PictureUrl { get; set; }

        public ProductItemOrdered Product { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        // we didn't add this property because we don't need to access the order from order item
        //public Order Order { get; set; } // Navigational Property 


        // Empty Parameterless Ctor -->
        // Entity Framework needs it to create an instance of the class when mapping the data from the database to the object.
        public OrderItem()
        {
            
        }


        // Ctor With Parameters -->
        // This constructor is used to create an instance of the OrderItem class with specific values for each property.
        public OrderItem(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }
    }
}
