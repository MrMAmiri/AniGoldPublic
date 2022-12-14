// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using AniGoldShop.Domain.Entities.Configurations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using AniGoldShop.Domain.Entities;
using System;
#nullable disable

#nullable disable

namespace AniGoldShop.Inferastructure.Data.DataContext
{
    public  class AnigoldContext : DbContext
    {
        public AnigoldContext()
        {
        }

        public AnigoldContext(DbContextOptions<AnigoldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agents> Agents { get; set; }
        public virtual DbSet<AgentZones> AgentZones { get; set; }
        public virtual DbSet<FactorItems> FactorItems { get; set; }
        public virtual DbSet<Factors> Factors { get; set; }
        public virtual DbSet<PaidInfos> PaidInfos { get; set; }
        public virtual DbSet<PayTypes> PayTypes { get; set; }
        public virtual DbSet<PriceTypes> PriceTypes { get; set; }
        public virtual DbSet<ProductGroups> ProductGroups { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<SendInfos> SendInfos { get; set; }
        public virtual DbSet<SendTypes> SendTypes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<Blogs> Blogs { get; set; }
        public virtual DbSet<BlogGroups> BlogGroups { get; set; }
        public virtual DbSet<Specials> Specials { get; set; }
        public virtual DbSet<Slides> Slides { get; set; }
        public virtual DbSet<Provinces> Provinces { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfiguration(new AgentsConfiguration());
            modelBuilder.ApplyConfiguration(new AgentZonesConfiguration());
            modelBuilder.ApplyConfiguration(new FactorItemsConfiguration());
            modelBuilder.ApplyConfiguration(new FactorsConfiguration());
            modelBuilder.ApplyConfiguration(new PaidInfosConfiguration());
            modelBuilder.ApplyConfiguration(new PayTypesConfiguration());
            modelBuilder.ApplyConfiguration(new PriceTypesConfiguration());
            modelBuilder.ApplyConfiguration(new ProductGroupsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
            modelBuilder.ApplyConfiguration(new SendInfosConfiguration());
            modelBuilder.ApplyConfiguration(new SendTypesConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new SettingsConfiguration());
            modelBuilder.ApplyConfiguration(new BlogsConfiguration());
            modelBuilder.ApplyConfiguration(new BlogGroupsConfiguration());
            modelBuilder.ApplyConfiguration(new SlidesConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialsConfiguration());
            modelBuilder.ApplyConfiguration(new ProvincesConfiguration());
            modelBuilder.ApplyConfiguration(new CitiesConfiguration());
        }

    }
}
