using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Domain.Interfaces;
using AniGoldShop.Domain.Interfaces.ExternalServices;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.UseCase.Blog.Query.Request;

namespace AniGoldShop.Application.UseCase.Blog.Query.Handler
{
    public class GetBlogsHandler : IRequestHandler<GetBlogsRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Blogs, Guid> _repository;

        public GetBlogsHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Blogs, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(GetBlogsRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {


                var resCount = await _repository.CountGODAsync(w =>
                (request.Name == null || w.BlogTitle.Contains(request.Name))
                &&
                (request.Tags == null || w.BlogTitle.Contains(request.Tags))
                &&
                (request.Group == null || w.BlogGroupId == request.Group)
                &&
                (request.Status == null || w.Status == request.Status)
                , request.PageSize.Value, request.PageNumber.Value);


                var res = await _repository.FindGODAsync(
                w =>
                (request.Name == null || w.BlogTitle.Contains(request.Name))
                &&
                (request.Tags == null || w.BlogTitle.Contains(request.Tags))
                &&
                (request.Group == null || w.BlogGroupId == request.Group)
                &&
                (request.Status == null || w.Status == request.Status)
                , o => o.CreateDate, true
                    , request.PageNumber.Value, request.PageSize.Value
                    ,i1=> i1.BlogGroup
                    ,i2=> i2.BlogAuthorNavigation);

                if (res != null && res.Any())
                {
                    funcresult.Data = new
                    {
                        data = res.Select(s => new
                        {
                            id = s.BlogId,
                            title=s.BlogTitle,
                            summary=s.BlogSummary,
                            text=s.BlogText,
                            images=s.BlogImages.CSplit(),
                            tags=s.BlogTags,
                            BlogGroupId=s.BlogGroupId,
                            BlogGroupName=s.BlogGroup?.BlogGroupName,
                            authorId=s.BlogAuthor,
                            authorName=s.BlogAuthorNavigation.UserFullName,
                            status = s.Status,
                            statusName = s.Status <= 0 ? "غیرفعال" : "فعال",
                            createDate = s.CreateDate.ToPersianDateString()
                        }).ToList(),
                        total = resCount.Item1,
                        pageCount = resCount.Item2,
                        nextPage = resCount.Item3
                        
                    };
                    funcresult.Message = "عملیات با موفقیت انجام شد";
                    funcresult.Successful = true;
                }
                else
                {
                    funcresult.Message = "موردی یافت نشد";
                }
            }
            catch (Exception ex)
            {
                funcresult.Message = ex.Message;
            }

            return funcresult;
        }

        
    }
}
