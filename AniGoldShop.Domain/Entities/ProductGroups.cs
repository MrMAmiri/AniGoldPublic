// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public class ProductGroups : BaseEntity
    {
        public ProductGroups()
        {
            InverseParent = new HashSet<ProductGroups>();
            Products = new HashSet<Products>();
        }

        public Guid ProductGroupId { get; set; }
        public Guid? ParentId { get; set; }
        public string ProductGroupName { get; set; }
        public string ProductGroupTitle { get; set; }
        public string ProductGroupIcon { get; set; }

        public virtual ProductGroups Parent { get; set; }
        public virtual ICollection<ProductGroups> InverseParent { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}