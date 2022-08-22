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
using AniGoldShop.Application.UseCase.Product.Query.Request;

namespace AniGoldShop.Application.UseCase.Product.Query.Handler
{
    public class GetProductsHandler : IRequestHandler<GetProductsRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Products, Guid> _repository;
        private IPriceAPIService _priceService;

        public GetProductsHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Products, Guid> repository,
            IPriceAPIService priceService
            )
        {
            _localize = localize;
            _repository = repository;
            _priceService = priceService;

        }
        public async Task<FuncResult> Handle(GetProductsRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                var pres = await _priceService.GetNewPrices();

                var resCount = await _repository.CountGODAsync(w =>
                (request.Id==null || w.ProductId==request.Id)
                &&
                (request.Name == null || w.ProductName.Contains(request.Name))
                &&
                (request.code == null || w.ProductCode==request.code)
                &&
                (request.group == null || w.ProductGroupId == request.group)
                &&
                (request.Special == null || w.Specials.Any(a => a.SpecialType == request.Special.Value
                                            && (a.SpecialStart == null || DateTime.Now.Date >= a.SpecialStart)
                                            && (a.SpecialEnd == null || DateTime.Now.Date <= a.SpecialEnd)
                                            ))
                &&
                (!request.OnlyDiscount || (w.ProductDiscount>0 
                                        && (w.ProductDiscountStart == null || DateTime.Now.Date >= w.ProductDiscountStart)
                                        && (w.ProductDiscountEnd == null || DateTime.Now.Date <= w.ProductDiscountEnd)))
                &&
                (request.Status == null || w.Status == request.Status)
                , request.PageSize.Value, request.PageNumber.Value);


                var res = await _repository.FindGODAsync(
                w =>
                (request.Id == null || w.ProductId == request.Id)
                &&
                (request.Name == null || w.ProductName.Contains(request.Name))
                &&
                (request.code == null || w.ProductCode == request.code)
                &&
                (request.group == null || w.ProductGroupId == request.group)
                &&
                (request.Special == null || w.Specials.Any(a => a.SpecialType == request.Special.Value
                                            && (a.SpecialStart == null || DateTime.Now.Date >= a.SpecialStart)
                                            && (a.SpecialEnd == null || DateTime.Now.Date <= a.SpecialEnd)
                                            ))
                &&
                (!request.OnlyDiscount || (w.ProductDiscount > 0
                                        && (w.ProductDiscountStart == null || DateTime.Now.Date >= w.ProductDiscountStart)
                                        && (w.ProductDiscountEnd == null || DateTime.Now.Date <= w.ProductDiscountEnd)))
                &&
                (request.Status == null || w.Status == request.Status)
                , o => o.CreateDate, true
                    , request.PageNumber.Value, request.PageSize.Value
                    ,i1=> i1.ProductGroup
                    ,i2=> i2.PriceType
                    ,i3=> i3.Specials);

                if (res != null && res.Any())
                {

                    var staticPrice = Guid.Parse("CB68A203-FE05-4EED-99E0-345B23EE2CBA");
                    var furmulaPrice = Guid.Parse("B6323DFF-3C6D-4C16-9EC5-2BF597C6D47C");
                    var calcPrice = Guid.Parse("D5191610-113D-437F-B314-0B2BBA6846C5");

                    Func<Guid, int> getPriceTypeNum = (x) =>
                    {

                        if (x == staticPrice)
                            return 0;
                        else if (x == furmulaPrice)
                            return 1;
                        else
                            return 2;

                    };

                    funcresult.Data = new
                    {
                        data = res.Select(s => new
                        {
                            id = s.ProductId,
                            name = s.ProductName,
                            title=s.ProductTitle,
                            desc=s.ProductDesc,
                            jsonDesc=s.ProductJsonDesc,
                            weight=s.ProductWeight,
                            cutting=s.ProductCutting,
                            priceType=s.PriceType.PriceTypeInfo,
                            priceTypeId=s.PriceTypeId,
                            priceInfo=s.ProductPriceInfo,
                            code=s.ProductCode,
                            images=s.ProductImages.CSplit(),
                            count=s.ProductCount,
                            discount=s.ProductDiscount,
                            discountStart=s.ProductDiscountStart?.ToPersianDateString(),
                            discountEnd = s.ProductDiscountEnd?.ToPersianDateString(),
                            productGroupId =s.ProductGroupId,
                            productGroupName=s.ProductGroup?.ProductGroupName,
                            productGroupPriority=(s.ProductGroup?.Priority==null)?0:s.ProductGroup?.Priority,
                            priceTypeNum = getPriceTypeNum(s.PriceTypeId),
                            mostSales=(s.Specials!=null 
                            && s.Specials
                                .Any(a=> a.SpecialType==1 
                                 && (a.SpecialStart==null || DateTime.Now.Date>=a.SpecialStart) 
                                 && (a.SpecialEnd == null || DateTime.Now.Date <= a.SpecialEnd))),
                            isNew = (s.Specials != null
                            && s.Specials
                                .Any(a => a.SpecialType == 2
                                 && (a.SpecialStart == null || DateTime.Now.Date >= a.SpecialStart)
                                 && (a.SpecialEnd == null || DateTime.Now.Date <= a.SpecialEnd))),
                            special = (s.Specials != null
                            && s.Specials
                                .Any(a => a.SpecialType == 3
                                 && (a.SpecialStart == null || DateTime.Now.Date >= a.SpecialStart)
                                 && (a.SpecialEnd == null || DateTime.Now.Date <= a.SpecialEnd))),
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
