﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public class PaidInfos : BaseEntity
    {
        public PaidInfos()
        {
            Factors = new HashSet<Factors>();
        }

        public Guid PaidInfoId { get; set; }

        public virtual ICollection<Factors> Factors { get; set; }
    }
}