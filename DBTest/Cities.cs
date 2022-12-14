// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public partial class Cities
    {
        public Cities()
        {
            AgentZones = new HashSet<AgentZones>();
            Factors = new HashSet<Factors>();
        }

        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public string CityName { get; set; }
        public string CityTitle { get; set; }
        public DateTime CreateDate { get; set; }
        public int? Status { get; set; }
        public Guid? CreateUser { get; set; }
        public Guid? ModifiedUser { get; set; }
        public long? Priority { get; set; }
        public string Lang { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Provinces Province { get; set; }
        public virtual ICollection<AgentZones> AgentZones { get; set; }
        public virtual ICollection<Factors> Factors { get; set; }
    }
}