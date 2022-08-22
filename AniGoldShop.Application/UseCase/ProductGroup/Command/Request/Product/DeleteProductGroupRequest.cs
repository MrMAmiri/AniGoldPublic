using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.ProductGroup.Command.Request
{
    public class DeleteProductGroupRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
    }
}
