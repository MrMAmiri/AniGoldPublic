using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Slide.Command.Request;
using AniGoldShop.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.Slide.Command.FluentValidation
{
    public class SlideFluentValidation : AbstractValidator<ModifySlideRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.Slides, Guid> _repository;

        public SlideFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Slides, Guid> repository)
        {
            _localize = localize;
            _repository = repository;

            RuleFor(e => e.Type)
                .NotNull()
                .WithMessage("نوع را وارد کنید");

            RuleFor(e => e.File)
                .NotNull()
                .WithMessage("فایل را وارد کنید");

            RuleFor(e => e)
                .Must(DifferDate)
                .WithMessage("تاریخ پایان نباید کوچکتر از تاریخ شروع باشد");
        }

        private bool DifferDate(ModifySlideRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.End)
                || string.IsNullOrWhiteSpace(request.Start))
                return true;


            if (request.Start.ToGeorgDate() > request.End.ToGeorgDate())
                return false;

            return true;

        }

    }
}
