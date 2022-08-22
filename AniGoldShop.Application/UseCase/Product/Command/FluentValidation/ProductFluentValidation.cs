using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Product.Command.Request;
using AniGoldShop.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.Product.Command.FluentValidation
{
    public class ProductFluentValidation : AbstractValidator<ModifyProductRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.Products, Guid> _repository;

        public ProductFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Products, Guid> repository)
        {
            _localize = localize;
            _repository = repository;

            RuleFor(e => e.Name)
                .NotNull()
                .WithMessage("نام را وارد کنید");

            RuleFor(e => e.Title)
                .NotNull()
                .WithMessage("عنوان را وارد کنید");

            RuleFor(e => e.PriceInfo)
                .NotNull()
                .WithMessage("قیمت را وارد کنید");

            RuleFor(e => e.Count)
                .NotNull()
                .WithMessage("تعداد را وارد کنید");

            RuleFor(e => e.Weight)
                .NotNull()
                .WithMessage("وزن را وارد کنید");

            RuleFor(e => e.Cutting)
                .NotNull()
                .WithMessage("عیار را وارد کنید");


            RuleFor(e => e.ProductGroupId)
                .NotNull()
                .WithMessage("گروه را وارد کنید");

            RuleFor(e => e)
                .MustAsync(NameIsValid)
                .WithMessage("نام تکراری است");

            RuleFor(e => e)
                .MustAsync(CodeIsValid)
                .WithMessage("کد تکراری است");

            RuleFor(e => e)
                .Must(DiscountDate)
                .WithMessage("تاریخ پایان تخفیف نباید کوچکتر از تاریخ شروع تخفیف باشد");

        }

        private async Task<bool> NameIsValid(ModifyProductRequest request, CancellationToken arg2)
        {
            //var res = await _repository.FindAsync(
            //    e => e.ProductName == request.Name && (request.Id==null || e.ProductId!=request.Id.Value));

            //if (res != null && res.Any())
            //    return false;
            //else
            return true;
        }
        private async Task<bool> CodeIsValid(ModifyProductRequest request, CancellationToken arg2)
        {
            decimal code = 1000;

            var res = await (_repository.MaxAsync(w => w.ProductCode));

            if (res > 0)
                code = res + 1;

            request.Code = (int)code;

            return true;
        }


        private bool DiscountDate(ModifyProductRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DiscountEnd)
                || string.IsNullOrWhiteSpace(request.DiscountStart))
                return true;


            if (request.DiscountStart.ToGeorgDate() > request.DiscountEnd.ToGeorgDate())
                return false;

            return true;

        }
    }
}
