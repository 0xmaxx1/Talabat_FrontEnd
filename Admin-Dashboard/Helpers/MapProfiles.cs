using Admin_Dashboard.Models;
using AutoMapper;
using Talabat.Core.Models.Product;

namespace Admin_Dashboard.Helpers
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
