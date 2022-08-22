using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Domain.Interfaces;
using AniGoldShop.Application.UseCase.User.Command.Request;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.User.Command.FluentValidation
{
    public class UserFluentValidation : AbstractValidator<ModifyUserRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.Users, Guid> _repository;

        public UserFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Users, Guid> repository)
        {
            _localize = localize;
            _repository = repository;

            RuleFor(e => e.FullName)
                .NotNull()
                .WithMessage("نام را وارد کنید");

            RuleFor(e => e.UserName)
                .NotNull()
                .WithMessage("نام کاربری را وارد کنید");

            RuleFor(e => e)
                .MustAsync(CheckPass)
                .WithMessage("رمز عبور را وارد کنید");

            RuleFor(e => e)
                .MustAsync(ConfirmPass)
                .WithMessage("رمز عبور با تکرار رمز برابر نیست");

            RuleFor(e => e)
                .MustAsync(NameIsValid)
                .WithMessage("نام کاربر تکراری است");            
            
            RuleFor(e => e)
                .MustAsync(CodeIsValid)
                .WithMessage("کد تکراری است");

        }

        private async Task<bool> NameIsValid(ModifyUserRequest request, CancellationToken arg2)
        {
            var res = await _repository.FindAsync(
                e => e.UserUsername == request.UserName && (request.Id==null || e.UserId!=request.Id.Value));

            if (res != null && res.Any())
                return false;
            else
                return true;
        }     
        private async Task<bool> CodeIsValid(ModifyUserRequest request, CancellationToken arg2)
        {
            var res = await _repository.FindAsync(
                e => e.UserCellPhone == request.CellPhone && (request.Id==null || e.UserId!=request.Id.Value));

            if (res != null && res.Any())
                return false;
            else
                return true;
        }

        private async Task<bool> CheckPass(ModifyUserRequest request, CancellationToken arg2)
        {
            if (request.Id == null)
            {
                if (string.IsNullOrWhiteSpace(request.Password))
                    return false;
            }

            return true;
        }

        private async Task<bool> ConfirmPass(ModifyUserRequest request, CancellationToken arg2)
        {
            if (request.Password != request.ConfirmPassword)
                return false;
            else
                return true;
        }


    }
}
