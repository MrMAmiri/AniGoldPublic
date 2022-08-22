using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Factor.Query.Request
{
    public class GetFactorsRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; } 
        public Guid? UserId { get; set; }
        public Guid? AgentId { get; set; }
        public int? Number { get; set; }
        public string CreateDate { get; set; }
        public bool? NotDeleted { get; set; } = false;

    }
}
