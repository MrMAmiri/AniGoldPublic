// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using AniGoldShop.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace AniGoldShop.Domain.Entities.Configurations
{
    public partial class SendTypesConfiguration : IEntityTypeConfiguration<SendTypes>
    {
        public void Configure(EntityTypeBuilder<SendTypes> entity)
        {
            entity.HasKey(e => e.SendTypeId);

            entity.Property(e => e.SendTypeId).HasDefaultValueSql("(newid())");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Lang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true);

            entity.Property(e => e.SendTypeName)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.SendTypeTitle)
                .IsRequired()
                .HasMaxLength(500);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SendTypes> entity);
    }
}
