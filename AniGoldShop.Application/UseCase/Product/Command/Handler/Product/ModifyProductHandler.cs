using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Product.Command.FluentValidation;
using AniGoldShop.Application.UseCase.Product.Command.Request;
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

namespace AniGoldShop.Application.UseCase.Product.Command.Handler
{
    public class ModifyProductHandler : IRequestHandler<ModifyProductRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Products, Guid> _repository;

        public ModifyProductHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Products, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(ModifyProductRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                var validator = new ProductFluentValidation(_localize, _repository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }

                var staticPrice = Guid.Parse("CB68A203-FE05-4EED-99E0-345B23EE2CBA");
                var furmulaPrice = Guid.Parse("B6323DFF-3C6D-4C16-9EC5-2BF597C6D47C");
                var calcPrice = Guid.Parse("D5191610-113D-437F-B314-0B2BBA6846C5");
               

                Domain.Entities.Products ent = null;
                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.ProductName = request.Name;
                    ent.ProductTitle = request.Title;
                    ent.ProductDesc = request.Desc;
                    ent.ProductJsonDesc = request.JsonDesc;
                    ent.ProductWeight = request.Weight;
                    ent.ProductCutting = request.Cutting;

                    if (request.PriceTypeNum == 0)
                        ent.PriceTypeId = staticPrice;
                    else if (request.PriceTypeNum == 1)
                        ent.PriceTypeId = furmulaPrice;
                    else if (request.PriceTypeNum == 2)
                        ent.PriceTypeId = calcPrice;

                    ent.ProductPriceInfo = request.PriceInfo;
                    ent.ProductCode = request.Code.Value;
                    ent.ProductGroupId = request.ProductGroupId;
                    ent.ProductImages = request.Images.CJoin();
                    ent.ProductDiscount = request.Discount;
                    ent.ProductDiscountStart = request.DiscountStart.ToGeorgDate();
                    ent.ProductDiscountEnd = request.DiscountEnd.ToGeorgDate();
                    ent.ProductCount = request.Count;
                    ent.Status = request.Status;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;
                    await _repository.Update(ent, true);
                }
                else
                {
                    ent = new Domain.Entities.Products()
                    {
                        ProductName = request.Name,
                        ProductTitle = request.Title,
                        ProductDesc = request.Desc,
                        ProductJsonDesc = request.JsonDesc,
                        ProductWeight = request.Weight,
                        ProductCutting = request.Cutting,
                        ProductPriceInfo = request.PriceInfo,
                        ProductCode = request.Code.Value,
                        ProductGroupId = request.ProductGroupId,
                        ProductImages = request.Images.CJoin(),
                        ProductCount = request.Count,
                        ProductDiscount = request.Discount,
                        ProductDiscountStart = request.DiscountStart.ToGeorgDate(),
                        ProductDiscountEnd = request.DiscountEnd.ToGeorgDate(),
                        ModifiedUser = request.Modifier,
                        Status = request.Status,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        ProductId = Guid.NewGuid()
                    };

                    if (request.PriceTypeNum == 0)
                        ent.PriceTypeId = staticPrice;
                    else if (request.PriceTypeNum == 1)
                        ent.PriceTypeId = furmulaPrice;
                    else if (request.PriceTypeNum == 2)
                        ent.PriceTypeId = calcPrice;

                    await _repository.Insert(ent, true);
                }

                funcresult.Message = "عملیات با موفقیت انجام شد";
                funcresult.Successful = true;
            }
            catch (Exception ex)
            {
                funcresult.Message = ex.Message;
            }

            return funcresult;
        }

    }
}
