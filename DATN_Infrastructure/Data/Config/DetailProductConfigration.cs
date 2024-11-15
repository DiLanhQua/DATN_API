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
    public class DetailProductConfigration : IEntityTypeConfiguration<DetailProduct>
    {
        public void Configure(EntityTypeBuilder<DetailProduct> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Size).HasMaxLength(5);
            builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
            builder.Property(x => x.Quantity);
            builder.Property(x => x.Gender).HasMaxLength(15);
            builder.Property(x => x.Status).HasMaxLength(15);
        }
    }
}
