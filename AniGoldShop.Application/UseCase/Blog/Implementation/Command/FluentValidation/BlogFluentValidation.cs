using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Blog.Command.Request;
using AniGoldShop.Domain.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AniGoldShop.Application.UseCase.Blog.Command.FluentValidation
{
    public class BlogFluentValidation : AbstractValidator<ModifyBlogRequest>
    {
        private readonly IStringLocalizer<TextLocalizationResource> _localize;
        private readonly IRepository<Domain.Entities.Blogs, Guid> _repository;

        public BlogFluentValidation(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Blogs, Guid> repository)
        {
            _localize = localize;
            _repository = repository;

            RuleFor(e => e.Title)
                .NotNull()
                .WithMessage("عنوان را وارد کنید");

            RuleFor(e => e.Summary)
                .NotNull()
                .WithMessage("خلاصه را وارد کنید");

            RuleFor(e => e)
                .MustAsync(NameIsValid)
                .WithMessage("عنوان تکراری است");            
            

        }

        private async Task<bool> NameIsValid(ModifyBlogRequest request, CancellationToken arg2)
        {
            var res = await _repository.FindAsync(
                e => e.BlogTitle == request.Title && (request.Id==null || e.BlogId!=request.Id.Value));

            if (res != null && res.Any())
                return false;
            else
                return true;
        }     


    }
}
