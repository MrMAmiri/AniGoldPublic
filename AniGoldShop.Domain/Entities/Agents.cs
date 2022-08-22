﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public class Agents:BaseEntity
    {
        public Agents()
        {
            AgentZones = new HashSet<AgentZones>();
            Factors = new HashSet<Factors>();
        }

        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public Guid UserRoleId { get; set; }


        public virtual UserRoles UserRole { get; set; }
        public virtual ICollection<AgentZones> AgentZones { get; set; }
        public virtual ICollection<Factors> Factors { get; set; }
    }
}