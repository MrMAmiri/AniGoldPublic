// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public  class AgentZones:BaseEntity
    {
        public Guid AgentZoneId { get; set; }
        public Guid AgentId { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public DateTime? AgentZoneStart { get; set; }
        public DateTime? AgentZoneEnd { get; set; }

        public virtual Agents Agent { get; set; }
        public virtual Cities City { get; set; }
        public virtual Provinces Province { get; set; }
    }
}