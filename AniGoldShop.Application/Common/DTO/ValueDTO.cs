namespace AniGoldShop.Application.Common.DTO
{
    public class ValueDTO<TResult> : BaseDTO
    {
        public TResult Result { get; set; }
    }
}
