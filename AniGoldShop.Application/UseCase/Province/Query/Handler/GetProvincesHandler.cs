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
using AniGoldShop.Application.UseCase.Province.Query.Request;

namespace AniGoldShop.Application.UseCase.Province.Query.Handler
{
    public class GetProvincesHandler : IRequestHandler<GetProvincesRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Provinces, Guid> _repository;
        private IPriceAPIService _priceService;

        public GetProvincesHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Provinces, Guid> repository,
            IPriceAPIService priceService
            )
        {
            _localize = localize;
            _repository = repository;
            _priceService = priceService;

        }
        public async Task<FuncResult> Handle(GetProvincesRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                var pres = await _priceService.GetNewPrices();

                var resCount = await _repository.CountGODAsync(w =>
                (request.Id==null || w.ProvinceId==request.Id)
                &&
                (request.Name == null || w.ProvinceName.Contains(request.Name))
                &&
                (request.Status == null || w.Status == request.Status)
                , request.PageSize.Value, request.PageNumber.Value);


                var res = await _repository.FindGODAsync(
                w =>
                (request.Id == null || w.ProvinceId == request.Id)
                &&
                (request.Name == null || w.ProvinceName.Contains(request.Name))
                &&
                (request.Status == null || w.Status == request.Status)
                , o => o.CreateDate, true
                    , request.PageNumber.Value, request.PageSize.Value
                    ,i1=> i1.Cities);

                if (res != null && res.Any())
                {
                    funcresult.Data = new
                    {
                        data = res.Select(s => new
                        {
                            id = s.ProvinceId,
                            name = s.ProvinceName,
                            title=s.ProvinceTitle,
                            cities=s.Cities.Select(c=> new
                            {
                                id=c.CityId,
                                provinceId=c.ProvinceId,
                                name=c.CityName,
                                title=c.CityTitle,
                                status=c.Status,
                                statusName= c.Status <= 0 ? "غیرفعال" : "فعال",
                                createDate = c.CreateDate.ToPersianDateString()
                            }).ToList(),
                            status = s.Status,
                            statusName = s.Status <= 0 ? "غیرفعال" : "فعال",
                            createDate = s.CreateDate.ToPersianDateString()
                        }).ToList(),
                        total = resCount.Item1,
                        pageCount = resCount.Item2,
                        nextPage = resCount.Item3,
                        prices = pres.ToList()
                        
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
