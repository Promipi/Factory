using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public sealed class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> entity)
        {
            entity.Property(e => e.Id)
                   .HasMaxLength(450)
                   .HasColumnType("NVARCHAR2");

            entity.Property(e => e.Date)
                .HasColumnType("TIMESTAMP"); // Ajusta según sea necesario

            entity.Property(e => e.Total)
                .HasColumnType("NUMBER(18, 2)");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(450)
                .HasColumnType("NVARCHAR2");

        }
    }
}
