﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using AniGoldShop.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace AniGoldShop.Domain.Entities.Configurations
{
    public partial class RolesConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> entity)
        {
            entity.HasKey(e => e.RoleId)
                .HasName("PK_Tbl_Roles");

            entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Lang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true);

            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(100);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Roles> entity);
    }
}
