using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Product;

namespace Talabat.Repository.Data.Config
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        // Apply Configurations for Product
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //builder.Property(P => P.Id).IsRequired();
            builder.Property(P => P.Name)/*.IsRequired()*/.HasMaxLength(100);
            //builder.Property(P => P.Description).IsRequired();
            builder.Property(P => P.Price).HasColumnType("decimal(18, 2)");
            //builder.Property(P => P.PictureURL).IsRequired();



            builder.HasOne(P => P.ProductBrand).WithMany() // it dosn't have Navigational Property from many side
                .HasForeignKey(P => P.ProductBrandId);


            builder.HasOne(P => P.ProductType).WithMany() // it dosn't have Navigational Property from Many side
                .HasForeignKey(P => P.ProductTypeId);
        }
    }
}
