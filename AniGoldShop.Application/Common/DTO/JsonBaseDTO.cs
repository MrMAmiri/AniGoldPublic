using AniGoldShop.Application.Common.Enum;

namespace AniGoldShop.Application.Common.DTO
{
    public abstract class JsonBaseDTO
    {
        public string Message { get; set; }
        public ResponseStatus Status { get; set; } 
        public string NextUrl { get; set; }
    }
}
