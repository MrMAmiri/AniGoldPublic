using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Factor.Command.Request
{
    public class DeleteFactorRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public bool? DeleteByStatus { get; set; } = false;
    }
}
