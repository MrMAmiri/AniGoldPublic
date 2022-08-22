using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Special.Command.Request
{
    public class DeleteSpecialRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? ProductId { get; set; }
        public short Type { get; set; }

        public bool? FourceDelete = false;
    }
}
