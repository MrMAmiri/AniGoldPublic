using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.Factor.Command.Request.Factor
{
    public class FactorItemRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public Guid? FactorId { get; set; }
        public Guid? ProductId { get; set; }
        public int? GQuantity { get; set; }
        public int? CQuantity { get; set; }
        public long? ProductPrice { get; set; }
        public byte? PercentDiscount { get; set; }
        public int? PriceDiscount { get; set; }
        public string CustomerDesc { get; set; }
        public string Desc { get; set; }

    }
}
