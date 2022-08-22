using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.User.Query.Request
{
    public class GetUserRolesRequest : BaseRequest, IRequest<FuncResult>
    {
        public string? Name { get; set; }
        public Guid? RoleId { get; set; }
        public bool OnlyUsers { get; set; } = false;
    }
}
