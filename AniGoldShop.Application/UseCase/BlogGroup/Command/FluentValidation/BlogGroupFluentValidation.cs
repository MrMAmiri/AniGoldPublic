using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.BlogGroup.Command.Request;
using AniGoldShop.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.BlogGroup.Command.FluentValidation
{
    public class BlogGroupFluentValidation : AbstractValidator<ModifyBlogGroupRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.BlogGroups, Guid> _repository;

        public BlogGroupFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.BlogGroups, Guid> repository)
        {
            _localize = localize;
            _repository = repository;

            RuleFor(e => e.Name)
                .NotNull()
                .WithMessage("نام را وارد کنید");

            RuleFor(e => e)
                .MustAsync(NameIsValid)
                .WithMessage("نام تکراری است");            
            

        }

        private async Task<bool> NameIsValid(ModifyBlogGroupRequest request, CancellationToken arg2)
        {
            var res = await _repository.FindAsync(
                e => e.BlogGroupName == request.Name && (request.Id==null || e.BlogGroupId!=request.Id.Value));

            if (res != null && res.Any())
                return false;
            else
                return true;
        }     


    }
}
