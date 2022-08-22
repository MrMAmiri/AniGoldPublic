using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.User.Command.Request
{
    public class ModifyUserRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public Guid? RoleId { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string UserName { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
