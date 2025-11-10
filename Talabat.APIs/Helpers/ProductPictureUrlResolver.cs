using AutoMapper;
using Talabat.Core.DTOs;
using Talabat.Core.Models.Product;

namespace Talabat.APIs.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureURL))
                return $"{_configuration["APIBaseURL"]}{source.PictureURL}";

            return string.Empty;
        }
    }
}
