using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Product.Command.Request
{
    public class ModifyProductRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public Guid ProductGroupId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string JsonDesc { get; set; }
        public string PriceInfo { get; set; }
        public string[] Images { get; set; }
        public decimal Weight { get; set; }
        public short Cutting { get; set; }
        public int? Code { get; set; }
        public int Count { get; set; }
        public int PriceTypeNum { get; set; }
        public long? Discount { get; set; } = 0;
        public string DiscountStart { get; set; }
        public string DiscountEnd { get; set; }

    }
}
