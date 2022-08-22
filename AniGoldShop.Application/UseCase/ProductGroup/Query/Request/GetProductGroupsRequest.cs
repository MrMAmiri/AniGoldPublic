using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.ProductGroup.Query.Request
{
    public class GetProductGroupsRequest : BaseRequest, IRequest<FuncResult>
    {
        public string Name { get; set; }
    }
}
