using DATN_Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.Config
{
    public class ColorConfigration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.NameColor).HasMaxLength(20);
            builder.Property(x => x.ColorCode).HasMaxLength(10);

        }
    }
}
