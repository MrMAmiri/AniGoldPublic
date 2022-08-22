using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Product.Query.Request
{
    public class GetProductsRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public int? code { get; set; }
        public Guid? group { get; set; }
        public short? Special { get; set; }
        public bool OnlyDiscount { get; set; } = false;
    }
}
