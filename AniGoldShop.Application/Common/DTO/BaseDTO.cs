using AniGoldShop.Application.Common.Enum;

namespace AniGoldShop.Application.Common.DTO
{
    public abstract class BaseDTO
    {
        public string Message { get; set; }
        public ResponseStatus Status { get; set; }
        public bool Successful { get; set; }
    }
}
