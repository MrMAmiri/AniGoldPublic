// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public  class FactorItems:BaseEntity
    {
        public Guid FactorItemId { get; set; }
        public int FactorItemCode { get; set; }
        public Guid FactorId { get; set; }
        public Guid ProductId { get; set; }
        public int? FactorItemGquantity { get; set; }
        public int? FactorItemCquantity { get; set; }
        public long FactorItemProductPrice { get; set; }
        public byte? FactorItemPercentDiscount { get; set; }
        public long? FactorItemPriceDiscount { get; set; }
        public string FactorItemCustomerDesc { get; set; }
        public string FactorItemSystemDesc { get; set; }

        public virtual Factors Factor { get; set; }
        public virtual Products Product { get; set; }
    }
}