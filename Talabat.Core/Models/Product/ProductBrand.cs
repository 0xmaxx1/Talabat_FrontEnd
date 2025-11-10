using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models.Product
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }

        // we didn't make Navigational Property for Product as we didn't need it
    }
}
