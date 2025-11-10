using Talabat.Core.Models;

namespace Talabat.Core.DTOs
{
    public class ProductToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public decimal Price { get; set; }

        // Navigational Properties
        public string ProductBrand { get; set; }
        //[ForeignKey(nameof(ProductBrand))]
        public int ProductBrandId { get; set; } // Foreign Key : Not allow Null


        public string ProductType { get; set; }
        //[ForeignKey(nameof(ProductType))]
        public int ProductTypeId { get; set; } // Foriegn Key : Not allow Null
    }
}
