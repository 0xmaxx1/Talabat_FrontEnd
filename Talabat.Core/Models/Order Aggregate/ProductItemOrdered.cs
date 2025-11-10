using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    // Class will be mapped with OrderItem
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }


        // Empty Parameterless Ctor -->
        // Entity Framework needs it to create an instance of the class when mapping the data from the database to the object.
        public ProductItemOrdered()
        {

        }

        // Ctor With Parameters -->
        // This constructor is used to create an instance of the ProductItemOrdered class with specific values for each property.
        public ProductItemOrdered(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }
    }
}
