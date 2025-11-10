using System.ComponentModel.DataAnnotations;
using Talabat.Core.Models.Product;

namespace Admin_Dashboard.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }


        public IFormFile? Image { get; set; }


        public string? PictureURL { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product BrandId is required")]
        public int ProductBrandId { get; set; }
        public ProductBrand? ProductBrand { get; set; }

        [Required(ErrorMessage = "Product TypeId is required")]
        public int ProductTypeId { get; set; }
        public ProductType? ProductType { get; set; }
    }
}
