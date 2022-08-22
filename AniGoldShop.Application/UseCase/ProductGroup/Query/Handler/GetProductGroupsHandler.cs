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
using AniGoldShop.Application.UseCase.ProductGroup.Query.Request;

namespace AniGoldShop.Application.UseCase.ProductGroup.Query.Handler
{
    public class GetProductGroupsHandler : IRequestHandler<GetProductGroupsRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.ProductGroups, Guid> _repository;

        public GetProductGroupsHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.ProductGroups, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(GetProductGroupsRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                var resCount = await _repository.CountGODAsync(w =>
                (request.Name == null || w.ProductGroupName.Contains(request.Name))
                &&
                (request.Status == null || w.Status == request.Status)
                , request.PageSize.Value, request.PageNumber.Value);


                var res = await _repository.FindGODAsync(
                w =>
                (request.Name == null || w.ProductGroupName.Contains(request.Name))
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
                            id = s.ProductGroupId,
                            name = s.ProductGroupName,
                            title=s.ProductGroupTitle,
                            icon=s.ProductGroupIcon,
                            status = s.Status,
                            priority=(s.Priority==null)?0:s.Priority,
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
