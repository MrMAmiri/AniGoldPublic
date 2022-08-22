using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using AniGoldShop.Application.UseCase.Factor.Command.Request.Factor;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Factor.Command.Request
{
    public class ModifyFactorRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? AgentId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public int? Number { get; set; }
        public Guid? PayTypeId { get; set; }
        public Guid? SendTypeId { get; set; }
        public Guid? FactorIsPaid { get; set; }
        public Guid? FactorIsSent { get; set; }
        public byte? PercentDiscount { get; set; }
        public long? PriceDiscount { get; set; }
        public string Desc { get; set; }
        public string CreateDate { get; set; }
        public List<FactorItemRequest> Items { get; set; }

    }
}
