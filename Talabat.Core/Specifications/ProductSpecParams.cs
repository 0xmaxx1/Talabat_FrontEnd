using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecParams
    {
        // Pagination Elements

        private const int maxPageSize = 10; // Maximum Page Size


        private int pageSize = 5; // Default Page Size
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > maxPageSize ? maxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;


        // Filtration Elements
        public string? Sort { get; set; }

        public int? BrandId { get; set; }

        public int? TypeId { get; set; }


        // Search Elements
        private string? search = string.Empty; // Default Search Value

        public string Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }

    }
}
