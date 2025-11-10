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
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // spread the address table into the order table
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner()); // 1:1 Total


            // Convert the enum OrderStatus to string for database storage
            builder.Property(O => O.Status)
                .HasConversion(
                OStatus => OStatus.ToString(),

                OStatusString => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatusString) // Convert the string back to the enum type
                              );


            // Specify the precision and scale for the decimal type
            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");


            // Change Delete Behavior
            builder.HasMany(O => O.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
