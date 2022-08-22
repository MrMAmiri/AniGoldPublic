using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Factor.Command.Request;
using AniGoldShop.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.Factor.Command.FluentValidation
{
    public class FactorFluentValidation : AbstractValidator<ModifyFactorRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.Factors, Guid> _repository;
        private readonly IRepository<Domain.Entities.FactorItems, Guid> _itemRepository;
        private byte generateCodeTry = 0;
        public FactorFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Factors, Guid> repository,
            IRepository<Domain.Entities.FactorItems, Guid> itemRepository)
        {
            _localize = localize;
            _repository = repository;

            RuleFor(e => e.UserId)
                .NotNull()
                .WithMessage("لطفا به کاربری خود وارد شوید");


            RuleFor(e => e.ProvinceId)
                .NotNull()
                .WithMessage("استان را انتخاب کنید");

            RuleFor(e => e.CityId)
                .NotNull()
                .WithMessage("شهر را انتخاب کنید");

            //RuleFor(e => e.CustomerAddressId)
            //    .NotNull()
            //    .WithMessage("آدرس را وارد کنید");


            RuleFor(e => e)
                .MustAsync(HasItem)
                .WithMessage("هیچ آیتمی وارد نشده");

            RuleFor(e => e)
                .MustAsync(CheckItems)
                .WithMessage("مشخصات آیتم وارد نشده");

            RuleFor(e => e)
                .MustAsync(DuplicateItem)
                .WithMessage("آیتم تکراری وارد شده است");

            RuleFor(e => e)
                .MustAsync(IsValid)
                .WithMessage("کد تکراری است");

        }

        private async Task<bool> IsValid(ModifyFactorRequest request, CancellationToken arg2)
        {
            if (generateCodeTry >= 3)
                return false;

            var code = int.Parse(
                 DateTime.Now.Month.ToString().PadLeft(2, '0') +
                 DateTime.Now.Day.ToString().PadLeft(2, '0') +
                 DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                 DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                 DateTime.Now.Second.ToString().PadLeft(2, '0')
                 );

            request.Number = code;
            var res = await _repository.FindAsync(
                e => e.FactorNumber == request.Number.Value
                && (request.Id == null || e.FactorId != request.Id.Value));

            if (res != null && res.Any())
            {
                generateCodeTry++;
                return await IsValid(request, arg2);
            }
            else
                return true;
        }

        private async Task<bool> HasItem(ModifyFactorRequest request, CancellationToken arg2)
        {
            if (request.Items == null || !request.Items.Any())
                return false;
            else
                return true;
        }

        private async Task<bool> CheckItems(ModifyFactorRequest request, CancellationToken arg2)
        {

            foreach (var item in request.Items)
            {
                if (
                    item.ProductId == null
                    || item.ProductId == null
                    || item.ProductPrice == null)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> DuplicateItem(ModifyFactorRequest request, CancellationToken arg2)
        {

            for (int i = 0; i < request.Items.Count; i++)
            {
                for (int b = 0; b < request.Items.Count; b++)
                {
                    if (b == i)
                        continue;

                    var x = request.Items[i];
                    var y = request.Items[b];

                    if (x.ProductId == y.ProductId)
                    {
                        return false;
                    }

                }
            }

            return true;
        }


    }
}
