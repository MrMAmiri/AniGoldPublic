﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public class SendTypes : BaseEntity
    {
        public SendTypes()
        {
            Factors = new HashSet<Factors>();
        }

        public Guid SendTypeId { get; set; }
        public string SendTypeName { get; set; }
        public string SendTypeTitle { get; set; }

        public virtual ICollection<Factors> Factors { get; set; }
    }
}