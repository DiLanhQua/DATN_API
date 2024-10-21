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
    public class DeliveryAddressConfigration : IEntityTypeConfiguration<DeliveryAddress>
    {
        public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(50);
            builder.Property(x => x.ZipCode);
            builder.Property(x => x.Phone).HasMaxLength(15);
            builder.Property(x => x.Note).HasMaxLength(100);
        }
    }
}
