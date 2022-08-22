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
using AniGoldShop.Application.UseCase.BlogGroup.Query.Request;

namespace AniGoldShop.Application.UseCase.BlogGroup.Query.Handler
{
    public class GetBlogGroupsHandler : IRequestHandler<GetBlogGroupsRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.BlogGroups, Guid> _repository;

        public GetBlogGroupsHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.BlogGroups, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(GetBlogGroupsRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                var resCount = await _repository.CountGODAsync(w =>
                (request.Name == null || w.BlogGroupName.Contains(request.Name))
                &&
                (request.Status == null || w.Status == request.Status)
                , request.PageSize.Value, request.PageNumber.Value);


                var res = await _repository.FindGODAsync(
                w =>
                (request.Name == null || w.BlogGroupName.Contains(request.Name))
                &&
                (request.Status == null || w.Status == request.Status)
                , o => o.CreateDate, true
                    , request.PageNumber.Value, request.PageSize.Value
                    , null);

                if (res != null && res.Any())
                {
                    funcresult.Data = new
                    {
                        data = res.Select(s => new
                        {
                            id = s.BlogGroupId,
                            name = s.BlogGroupName,
                            title=s.BlogGroupTitle,
                            icon=s.BlogGroupIcon,
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
