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
    public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .HasColumnType("NVARCHAR2");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.Description)
                .HasMaxLength(2000)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(18, 2)");

            entity.Property(e => e.Category)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");
        }
    }
}
