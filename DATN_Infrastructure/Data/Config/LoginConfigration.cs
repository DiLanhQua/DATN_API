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
    public class LoginConfigration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Action).HasMaxLength(50);
            builder.Property(x => x.TimeStamp);
            builder.Property(x => x.Description).HasMaxLength(255);
        }
    }
}
