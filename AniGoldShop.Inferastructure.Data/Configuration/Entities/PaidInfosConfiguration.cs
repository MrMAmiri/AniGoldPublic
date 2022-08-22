﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using AniGoldShop.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using AniGoldShop.Inferastructure.Data.Common.Configuration;
namespace AniGoldShop.Domain.Entities.Configurations
{
    public class PaidInfosConfiguration  : BaseEntityConfiguration<PaidInfos>
    {
        public override void Configure(EntityTypeBuilder<PaidInfos> entity)
        {
            entity.HasKey(e => e.PaidInfoId);

            entity.Property(e => e.PaidInfoId).HasDefaultValueSql("(newid())");

            entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Lang)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength(true);

            
        }

 
    }
}
