using App.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Data.Configurations
{
    public class BusStopConfiguration : IEntityTypeConfiguration<BusStop>
    {
        public void Configure(EntityTypeBuilder<BusStop> builder)
        {
            builder.Property(p => p.Address)
                .IsRequired();
        }
    }
}
