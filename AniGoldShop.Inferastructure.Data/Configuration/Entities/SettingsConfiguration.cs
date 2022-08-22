﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using AniGoldShop.Inferastructure.Data.Common.Configuration;

namespace AniGoldShop.Domain.Entities.Configurations
{
    public class SettingsConfiguration : BaseEntityConfiguration<Settings>
    {
        public override void Configure(EntityTypeBuilder<Settings> entity)
        {
            entity.HasKey(e => e.SettingId);

            entity.Property(e => e.SettingId).ValueGeneratedNever();

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Lang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true);

            entity.Property(e => e.SettingKey)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.SettingTitle)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.SettingValue)
                .IsRequired()
                .HasMaxLength(250);

        }

    }
}
