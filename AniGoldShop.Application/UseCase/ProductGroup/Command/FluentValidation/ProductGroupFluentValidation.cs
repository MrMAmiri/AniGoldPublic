using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.ProductGroup.Command.Request;
using AniGoldShop.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.ProductGroup.Command.FluentValidation
{
    public class ProductGroupFluentValidation : AbstractValidator<ModifyProductGroupRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.ProductGroups, Guid> _repository;

        public ProductGroupFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.ProductGroups, Guid> repository)
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

        private async Task<bool> NameIsValid(ModifyProductGroupRequest request, CancellationToken arg2)
        {
            var res = await _repository.FindAsync(
                e => e.ProductGroupName == request.Name && (request.Id==null || e.ProductGroupId!=request.Id.Value));

            if (res != null && res.Any())
                return false;
            else
                return true;
        }     


    }
}
