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
using AniGoldShop.Application.UseCase.Factor.Query.Request;
using AniGoldShop.Application.Common.Enum;

namespace AniGoldShop.Application.UseCase.Factor.Query.Handler
{
    public class GetFactorsHandler : IRequestHandler<GetFactorsRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Factors, Guid> _repository;
        private IRepository<Domain.Entities.FactorItems, Guid> _itemRepository;
        private IRepository<Domain.Entities.Users, Guid> _userRepo;
        private IRepository<Domain.Entities.UserRoles, Guid> _userRoleRepo;
        private IRepository<Domain.Entities.Roles, Guid> _roleRepo;

        public GetFactorsHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Factors, Guid> repository,
            IRepository<Domain.Entities.FactorItems, Guid> itemRepository,
            IRepository<Domain.Entities.Users, Guid> userRepo,
            IRepository<Domain.Entities.UserRoles, Guid> userRoleRepo,
            IRepository<Domain.Entities.Roles, Guid> roleRepo
            )
        {
            _localize = localize;
            _repository = repository;
            _itemRepository = itemRepository;
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _roleRepo = roleRepo;

        }
        public async Task<FuncResult> Handle(GetFactorsRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                await CheckPerms(request, cancellationToken);

                var resCount = await _repository.CountGODAsync(w =>
                (request.Id==null || w.FactorId==request.Id)
                &&
                (request.UserId == null || w.UserId == request.UserId)
                &&
                (request.AgentId == null || w.AgentId == request.AgentId)
                &&
                (request.Number == null || w.FactorNumber == request.Number)
                &&
                (string.IsNullOrWhiteSpace(request.FromDate) || w.CreateDate >= request.FromDate.ToGeorgDate())
                &&
                (string.IsNullOrWhiteSpace(request.ToDate) || w.CreateDate <= request.ToDate.ToGeorgDate())
                &&
                (request.Status == null || w.Status == request.Status)
                &&
                (request.NotDeleted == null || (!request.NotDeleted.Value && 1 == 1) || (request.NotDeleted.Value && w.Status != 100))
                , request.PageSize.Value, request.PageNumber.Value);



                var res = await _repository.FindGODAsync(w =>
                                (request.Id == null || w.FactorId == request.Id)
                &&
                (request.UserId == null || w.UserId == request.UserId)
                &&
                (request.AgentId == null || w.AgentId == request.AgentId)
                &&
                (request.Number == null || w.FactorNumber == request.Number)
                &&
                (string.IsNullOrWhiteSpace(request.FromDate) || w.CreateDate >= request.FromDate.ToGeorgDate())
                &&
                (string.IsNullOrWhiteSpace(request.ToDate) || w.CreateDate <= request.ToDate.ToGeorgDate())
                &&
                (request.Status == null || w.Status == request.Status)
                &&
                (request.NotDeleted == null || (!request.NotDeleted.Value && 1==1) || (request.NotDeleted.Value && w.Status!=100))
                , o => o.CreateDate, true
                    , request.PageNumber.Value, request.PageSize.Value
                    ,i1=> i1.User
                    ,i2=> i2.Province
                    ,i3=> i3.City);

                if (res != null && res.Any())
                {
                    res = res.ToList();

                    foreach(var item in res)
                    {
                        item.FactorItems = (await _itemRepository.FindAsync(w => w.FactorId == item.FactorId,
                            i1 => i1.Product)).ToList();
                    }

                    funcresult.Data = new
                    {

                        data = res.Select((o, ind) => new
                        {
                            rowId = ind + 1,
                            selected = false,
                            id = o.FactorId,
                            userId = o.UserId,
                            userName=o.User.UserFullName,
                            number = o.FactorNumber,
                            provinceId = o.ProvinceId,
                            cityId = o.CityId,
                            provinceName = o.Province.ProvinceTitle,
                            cityName = o.City.CityTitle,
                            agentId = o.AgentId,
                            address = o.FactorAddress,
                            cellphone = o.FactorCellPhone,
                            payTypeId = o.PayTypeId,
                            sendTypeId = o.SendTypeId,
                            factorIsPaid = o.FactorIsPaid,
                            factorIsSent = o.FactorIsSent,
                            percentDiscount = o.FactorPercentDiscount,
                            factorPriceDiscount = o.FactorPriceDiscount,
                            desc = o.FactorDesc,
                            status = o.Status,
                            statusName = StaticMethods.GetFactorStatus(o.Status),
                            createDate = o.CreateDate.ToPersianDateString(true),
                            priority = o.Priority,
                            itemsCount=o.FactorItems.Sum(ss=> ss.FactorItemCquantity),
                            totalPrice=o.FactorItems.Sum(ss=> ss.FactorItemProductPrice*ss.FactorItemCquantity),
                            items = o.FactorItems.Select((i,rid)=> new {
                                rowId = rid + 1,
                                selected = false,
                                id =i.FactorItemId,
                                factorId=i.FactorId,
                                code=i.FactorItemCode,
                                productId=i.ProductId,
                                productTitle=i.Product.ProductTitle+"("+i.Product.ProductCode+")",
                                gQuantity=i.FactorItemGquantity,
                                cQuantity=i.FactorItemCquantity,
                                price=i.FactorItemProductPrice,
                                percentDiscount=i.FactorItemPercentDiscount,
                                priceDiscount=i.FactorItemPriceDiscount,
                                customerDesc=i.FactorItemCustomerDesc,
                                systemDesc= i.FactorItemSystemDesc,
                                createDate = o.CreateDate.ToPersianDateString(true),
                                status = o.Status,
                                statusName = o.Status == 0 ? "جدید" : "",

                            }).ToList()

                        }).ToList(),
                        total = resCount.Item1,
                        pageCount = resCount.Item2,
                        nextPage = resCount.Item3
                    };

                    funcresult.Successful = true;
                    funcresult.Message = "عملیات با موفقیت انجام شد";
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



        async Task<string> GetUser(Guid roleId, Guid userId, CancellationToken cancellationToken)
        {
            string user = "";
            try
            {

                var res = await _userRepo.FindGODAsync(
                    w => w.UserId == userId, includes: i1 => i1.UserRoles);

                var roleRes = await _roleRepo.FindGODAsync(
                    w => w.RoleId == roleId);

                if (res != null && res.Any())
                {
                    user =
                        res.FirstOrDefault().UserFullName + '(' + roleRes.FirstOrDefault().RoleName + ')';
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }

            return user;
        }

        async Task CheckPerms(GetFactorsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var rr = (await _userRoleRepo.FindAsync(w => w.UserRoleId == request.Modifier,i1=> i1.Agents));

                if(rr != null)
                {
                    if(rr.Any(w=> w.RoleId== Guid.Parse(StaticMethods.CustomerRole)))
                    {
                        request.UserId = rr.FirstOrDefault().UserId;
                        request.NotDeleted = true;
                    }

                    if(rr.Any(w=> w.RoleId == Guid.Parse(StaticMethods.AgentRole)))
                    {
                        request.AgentId = rr.FirstOrDefault().Agents.FirstOrDefault().AgentId;
                    }

                }
            }
            catch (Exception ex)
            {
            }

        }
    }
}
