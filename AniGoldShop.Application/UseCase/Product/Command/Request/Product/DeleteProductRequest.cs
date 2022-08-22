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
    public class DeleteProductRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }

        public bool ForceDelete { get; set; } = false;
    }
}
