using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Province.Query.Request
{
    public class GetProvincesRequest : BaseRequest, IRequest<FuncResult>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
