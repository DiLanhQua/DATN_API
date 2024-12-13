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
    public class VoucherConfigration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.VoucherName).HasMaxLength(100);
            builder.Property(x => x.TimeStart);
            builder.Property(x => x.TimeEnd);
            builder.Property(x => x.DiscountType);
            builder.Property(x => x.Discount).HasColumnType("decimal(10,2)");
            builder.Property(x => x.Quantity);
            builder.Property(x => x.Min_Order_Value).HasColumnType("decimal(10,2)");
            builder.Property(x => x.Max_Discount).HasColumnType("decimal(10,2)");
            builder.Property(x => x.Status);
        }
    }
}
