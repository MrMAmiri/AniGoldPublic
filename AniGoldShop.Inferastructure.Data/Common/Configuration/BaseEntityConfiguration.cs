using AniGoldShop.Domain.Common.BaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Inferastructure.Data.Common.Configuration
{
    public class BaseEntityConfiguration<TEntity>
       : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreateDate).HasColumnType("datetime2(7)");
            builder.Property(e => e.ModifiedDate).HasColumnType("datetime2(7)");
            builder.Property(e => e.Lang)
                             .HasMaxLength(2)
                             .IsUnicode(false)
                             .IsFixedLength(true);
        }
    }
}
