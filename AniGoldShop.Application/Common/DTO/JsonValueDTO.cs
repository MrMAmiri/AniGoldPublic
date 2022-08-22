using AniGoldShop.Application.Common.Enum;

namespace AniGoldShop.Application.Common.DTO
{
    public abstract class JsonValueDTO
    {
        public string Message { get; set; }
        public ResponseStatus Status { get; set; }
        public string NextUrl { get; set; }
        public object Data { get; set; }
    }
}
