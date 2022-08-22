using AniGoldShop.Application;
using AniGoldShop.Application.Common.ViewModel;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.User.Command.Request
{
    public  class LoginRequest : IRequest<FuncResult>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CellPhone { get; set; }
        public string Address { get; set; }
        public bool CreateIfNot { get; set; } = false;
        public bool OnlyCustomer { get; set; } = false;
        public JWTSettings? jwtSettings { get; set; }

    }
}
