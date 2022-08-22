using System;
using System.ComponentModel.DataAnnotations;

namespace AniGoldShop.Domain.Common.BaseModels
{
    public abstract class BaseEntity
    {
        public DateTime CreateDate { get; set; }
        public int? Status { get; set; }
        public Guid? CreateUser { get; set; }
        public Guid? ModifiedUser { get; set; }
        public long? Priority { get; set; }
        
        [StringLength(2)]
        public string Lang { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
