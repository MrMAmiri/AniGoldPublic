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
using AniGoldShop.Application.UseCase.Slide.Query.Request;

namespace AniGoldShop.Application.UseCase.Slide.Query.Handler
{
    public class GetSlidesHandler : IRequestHandler<GetSlidesRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Slides, Guid> _repository;

        public GetSlidesHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Slides, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(GetSlidesRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {


                var resCount = await _repository.CountGODAsync(w =>
                (request.Type == null || w.SlideType == request.Type)
                &&
                (!request.OnlyActives || w.SlideStart==null || DateTime.Now.Date>=w.SlideStart.Value)
                &&
                (!request.OnlyActives || w.SlideEnd==null || DateTime.Now.Date <= w.SlideEnd.Value)
                &&
                (request.FileType == null || w.SlideFileType == request.FileType)
                &&
                (request.Status == null || w.Status == request.Status)
                , request.PageSize.Value, request.PageNumber.Value);


                var res = await _repository.FindGODAsync(
                w =>
                (request.Type == null || w.SlideType == request.Type)
                &&
                (!request.OnlyActives || w.SlideStart == null || DateTime.Now.Date >= w.SlideStart.Value)
                &&
                (!request.OnlyActives || w.SlideEnd == null || DateTime.Now.Date <= w.SlideEnd.Value)
                &&
                (request.FileType == null || w.SlideFileType == request.FileType)
                &&
                (request.Status == null || w.Status == request.Status)
                , o => o.CreateDate, true
                    , request.PageNumber.Value, request.PageSize.Value, null);

                if (res != null && res.Any())
                {
                    funcresult.Data = new
                    {
                        data = res.Select(s => new
                        {
                            id = s.SlideId,
                            type = s.SlideType,
                            typeName = s.SlideType == 2 ? "وسط" : "بالا",
                            start = s.SlideStart?.ToPersianDateString(),
                            end = s.SlideEnd?.ToPersianDateString(),
                            fileType = s.SlideFileType,
                            file = s.SlideFile,
                            link = s.SlideLink,
                            html = s.SlideHtml,
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
