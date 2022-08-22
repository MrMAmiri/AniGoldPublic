﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public partial class Factors
    {
        public Factors()
        {
            FactorItems = new HashSet<FactorItems>();
        }

        public Guid FactorId { get; set; }
        public Guid UserId { get; set; }
        public int FactorNumber { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public Guid? AgentId { get; set; }
        public string FactorAddress { get; set; }
        public string FactorCellPhone { get; set; }
        public Guid PayTypeId { get; set; }
        public Guid SendTypeId { get; set; }
        public Guid? FactorIsPaid { get; set; }
        public Guid? FactorIsSent { get; set; }
        public byte? FactorPercentDiscount { get; set; }
        public long? FactorPriceDiscount { get; set; }
        public string FactorDesc { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Status { get; set; }
        public Guid? CreateUser { get; set; }
        public Guid? ModifiedUser { get; set; }
        public long? Priority { get; set; }
        public string Lang { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Agents Agent { get; set; }
        public virtual Cities City { get; set; }
        public virtual PaidInfos FactorIsPa { get; set; }
        public virtual SendInfos FactorIsSentNavigation { get; set; }
        public virtual PayTypes PayType { get; set; }
        public virtual Provinces Province { get; set; }
        public virtual SendTypes SendType { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<FactorItems> FactorItems { get; set; }
    }
}