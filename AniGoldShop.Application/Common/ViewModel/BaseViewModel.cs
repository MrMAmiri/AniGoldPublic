using System;

namespace AniGoldShop.Application.Common.ViewModel
{
    public abstract class BaseViewModel<TEntity> : ICloneable
    {
        public TEntity Entity { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
