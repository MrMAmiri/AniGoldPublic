using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Special.Command.Request;
using AniGoldShop.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.Special.Command.FluentValidation
{
    public class SpecialFluentValidation : AbstractValidator<ModifySpecialRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.Specials, Guid> _repository;

        public SpecialFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Specials, Guid> repository)
        {
            _localize = localize;
            _repository = repository;

            RuleFor(e => e.ProductId)
                .NotNull()
                .WithMessage("محصول را وارد کنید");

            RuleFor(e => e)
                .MustAsync(IsDuplicate)
                .WithMessage("تکراری است");            
            

        }

        private async Task<bool> IsDuplicate(ModifySpecialRequest request, CancellationToken arg2)
        {

            var res = await _repository.FindAsync(
                e => e.ProductId == request.ProductId && e.SpecialType==request.Type
                && (request.Id==null || e.SpecialId!=request.Id.Value));

            if (res != null && res.Any())
                return false;
            else
                return true;
        }     


    }
}
