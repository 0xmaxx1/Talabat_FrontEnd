using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Product
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public decimal Price { get; set; }

        // Navigational Properties
        public ProductBrand ProductBrand { get; set; } // Navigational Property --> [One]
        //[ForeignKey(nameof(ProductBrand))]
        public int ProductBrandId { get; set; } // Foreign Key : Not allow Null


        public ProductType ProductType { get; set; } // Navigational Property [One]
        //[ForeignKey(nameof(ProductType))]
        public int ProductTypeId { get; set; } // Foriegn Key : Not allow Null
    }
}
