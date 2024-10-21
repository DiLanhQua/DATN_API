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
    public class BlogConfigration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.HeadLine).HasMaxLength(100);
            builder.Property(x => x.Content).HasMaxLength(255);
            builder.Property(x => x.DatePush);
        }
    }
}
