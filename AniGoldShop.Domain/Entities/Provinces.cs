// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public  class Provinces:BaseEntity
    {
        public Provinces()
        {
            AgentZones = new HashSet<AgentZones>();
            Cities = new HashSet<Cities>();
            Factors = new HashSet<Factors>();
        }

        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceTitle { get; set; }

        public virtual ICollection<AgentZones> AgentZones { get; set; }
        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<Factors> Factors { get; set; }
    }
}