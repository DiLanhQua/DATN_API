﻿using DATN_Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Infrastructure.Data.Config
{
    public class DetailCartConfigration : IEntityTypeConfiguration<DetailCart>
    {
        public void Configure(EntityTypeBuilder<DetailCart> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Quantity);
        }

    }
}
