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
    public sealed class ProductSaleConfiguration : IEntityTypeConfiguration<ProductSale>
    {
        public void Configure(EntityTypeBuilder<ProductSale> entity)
        {
            entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .HasColumnType("NVARCHAR2");

            entity.Property(e => e.ProductId)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.SaleId)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.SubTotal)
                .HasColumnType("NUMBER(18, 2)");

            entity.HasOne(ps => ps.Product)
                .WithMany()
                .HasForeignKey(ps => ps.ProductId);

        }
    }
}
