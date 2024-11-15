using DATN_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.Config
{
    public class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Total).HasColumnType("decimal(10,2)");
            builder.Property(x => x.TimeOrder);
            builder.Property(x => x.StatusOrder);
            builder.Property(x => x.PaymentMethod).HasMaxLength(25);
                
        }
    }
}
