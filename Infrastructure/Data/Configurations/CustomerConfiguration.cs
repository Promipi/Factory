using Common.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {

        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            entity.Property(e => e.Id)
                    .HasMaxLength(450)
                    .HasColumnType("NVARCHAR2");

            entity.Property(e => e.UserName)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.NormalizedUserName)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.Email)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.NormalizedEmail)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.FirstName)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.LastName)
                .HasMaxLength(256)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.RefreshToken)
                .HasMaxLength(2000)
                .HasColumnType("NVARCHAR2");

            entity.Property(e => e.RefreshTokenExpiryTime)
                .HasColumnType("TIMESTAMP"); 

            entity.Property(e => e.EmailConfirmed)
                .HasColumnType("NUMBER(1)")
                .IsRequired();

            entity.Property(e => e.PhoneNumberConfirmed)
                .HasColumnType("NUMBER(1)")
                .IsRequired();

            entity.Property(e => e.TwoFactorEnabled)
                .HasColumnType("NUMBER(1)")
                .IsRequired();

            entity.Property(e => e.LockoutEnabled)
                .HasColumnType("NUMBER(1)")
                .IsRequired();

            entity.Property(e => e.LockoutEnd)
                .HasColumnType("TIMESTAMP WITH TIME ZONE");

            entity.Property(e => e.AccessFailedCount)
                .HasColumnType("NUMBER(10)");

            entity.HasMany(e => e.Purchases)
                .WithOne(ps => ps.Customer)
                .HasForeignKey(ps => ps.CustomerId);
        }
    }
}
