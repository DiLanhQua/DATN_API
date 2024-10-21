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
    public class DetailOrderConfigration : IEntityTypeConfiguration<DetailOrder>
    {
        public void Configure(EntityTypeBuilder<DetailOrder> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Quantity);
        }
    }
}
