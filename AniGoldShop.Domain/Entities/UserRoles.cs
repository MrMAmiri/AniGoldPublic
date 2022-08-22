﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using AniGoldShop.Domain.Common.BaseModels;
using System;
using System.Collections.Generic;

namespace AniGoldShop.Domain.Entities
{
    public class UserRoles : BaseEntity
    {
        public UserRoles()
        {
            Agents = new HashSet<Agents>();
        }

        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Roles Role { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<Agents> Agents { get; set; }
    }
}