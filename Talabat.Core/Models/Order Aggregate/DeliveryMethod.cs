using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Order_Aggregate
{
    public class DeliveryMethod : BaseEntity
    {
        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DeliveryTime { get; set; }

        // ONE --> Order // we don't need to add this property because we don't need to access the order from delivery method

        // Empty Parameterless Ctor -->
        // Entity Framework needs it to create an instance of the class when mapping the data from the database to the object.
        public DeliveryMethod()
        {
            
        }
        // Ctor with Parameters -->
        // This constructor is used to create an instance of the DeliveryMethod class with specific values for each property.
        public DeliveryMethod(string shortName, string description, decimal cost, string deliveryTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }
    }
}
