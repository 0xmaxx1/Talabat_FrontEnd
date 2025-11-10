using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // spread the product table into the order item table
            builder.OwnsOne(OI => OI.Product, P => P.WithOwner());


            // Specify the precision and scale for the decimal type
            builder.Property(OI => OI.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
