using AniGoldShop.Application.Common.Enum;
using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Factor.Command.FluentValidation;
using AniGoldShop.Application.UseCase.Factor.Command.Request;
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

namespace AniGoldShop.Application.UseCase.Factor.Command.Handler
{
    public class ModifyFactorHandler : IRequestHandler<ModifyFactorRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Factors, Guid> _repository;
        private IRepository<Domain.Entities.FactorItems, Guid> _itemRepository;
        private IRepository<Domain.Entities.UserRoles, Guid> _usrRoleRepository;
        private IRepository<Domain.Entities.Agents, Guid> _agentRepository;

        public ModifyFactorHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Factors, Guid> repository,
            IRepository<Domain.Entities.FactorItems, Guid> itemRepository,
            IRepository<Domain.Entities.UserRoles, Guid> usrRoleRepository,
            IRepository<Domain.Entities.Agents, Guid> agentRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _itemRepository = itemRepository;
            _usrRoleRepository = usrRoleRepository;
            _agentRepository = agentRepository;

        }
        public async Task<FuncResult> Handle(ModifyFactorRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                request.UserId = (await _usrRoleRepository.FindAsync(w => w.UserRoleId == request.Modifier))
                    .FirstOrDefault().UserId;


                var validator = new FactorFluentValidation(_localize, _repository, _itemRepository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }



                await SetAgent(request, cancellationToken);


                Domain.Entities.Factors ent = null;

                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.UserId = request.UserId.Value;
                    ent.FactorNumber = request.Number.Value;
                    ent.ProvinceId = request.ProvinceId;
                    ent.CityId = request.CityId;
                    ent.FactorDesc = request.Desc;
                    ent.AgentId = request.AgentId;
                    ent.FactorAddress = request.Address;
                    ent.FactorCellPhone = request.CellPhone;
                    ent.PayTypeId = request.PayTypeId.Value;
                    ent.SendTypeId = request.SendTypeId.Value;
                    ent.FactorIsPaid = request.FactorIsPaid;
                    ent.FactorIsSent = request.FactorIsSent;
                    ent.FactorPercentDiscount = request.PercentDiscount;
                    ent.FactorPriceDiscount = request.PriceDiscount;
                    ent.Status = 0;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;
                    await _itemRepository.Delete(w => w.FactorId == request.Id.Value);

                    await _itemRepository.InsertRange(request.Items.Select(c => new Domain.Entities.FactorItems()
                    {
                        FactorId = c.FactorId.Value,
                        FactorItemCustomerDesc = c.CustomerDesc,
                        FactorItemId = Guid.NewGuid(),
                        FactorItemCode = request.Number.Value,
                        FactorItemCquantity = c.CQuantity,
                        FactorItemGquantity = c.GQuantity,
                        FactorItemPriceDiscount = c.PriceDiscount,
                        FactorItemPercentDiscount = c.PercentDiscount,
                        FactorItemProductPrice = c.ProductPrice.Value,
                        FactorItemSystemDesc = c.Desc,
                        ProductId = c.ProductId.Value,
                        Status = 0,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = request.Modifier,
                        CreateUser = request.Modifier

                    }));

                    await _repository.Update(ent, true);
                }
                else
                {
                    var fid = Guid.NewGuid();
                    ent = new Domain.Entities.Factors()
                    {
                        UserId = request.UserId.Value,
                        FactorNumber = request.Number.Value,
                        ProvinceId = request.ProvinceId,
                        CityId = request.CityId,
                        FactorDesc = request.Desc,
                        AgentId = request.AgentId,
                        FactorAddress = request.Address,
                        FactorCellPhone = request.CellPhone,
                        PayTypeId = request.PayTypeId.Value,
                        SendTypeId = request.SendTypeId.Value,
                        FactorIsPaid = request.FactorIsPaid,
                        FactorIsSent = request.FactorIsSent,
                        FactorPercentDiscount = request.PercentDiscount,
                        FactorPriceDiscount = request.PriceDiscount,
                        Status = 0,
                        ModifiedUser = request.Modifier,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        FactorId = fid
                    };
                    await _repository.Insert(ent);



                    await _itemRepository.InsertRange(request.Items.Select((c, idx) => new Domain.Entities.FactorItems()
                    {
                        FactorId = fid,
                        FactorItemCustomerDesc = c.CustomerDesc,
                        FactorItemId = Guid.NewGuid(),
                        FactorItemCode = request.Number.Value,
                        FactorItemCquantity = c.CQuantity,
                        FactorItemGquantity = c.GQuantity,
                        FactorItemPriceDiscount = c.PriceDiscount,
                        FactorItemPercentDiscount = c.PercentDiscount,
                        FactorItemProductPrice = c.ProductPrice.Value,
                        FactorItemSystemDesc = c.Desc,
                        ProductId = c.ProductId.Value,
                        Status = 0,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = request.Modifier,
                        CreateUser = request.Modifier

                    }), true);
                }

                funcresult.Data = request.Number;
                funcresult.Message = "عملیات با موفقیت انجام شد";
                funcresult.Successful = true;
            }
            catch (Exception ex)
            {
                funcresult.Message = "خطای سیستمی، لطفا فیلد ها را تکمیل کنید";
            }

            return funcresult;
        }


        private async Task SetAgent(ModifyFactorRequest request,CancellationToken cancellation)
        {
            var agg = (await _agentRepository
                .FindAsync(
                w => w.AgentZones.Any(
                    f => f.ProvinceId == request.ProvinceId && (f.City == null || f.CityId == request.CityId
                    && f.Status == 1)))).FirstOrDefault();

            if (agg != null)
            {
                request.AgentId = agg.AgentId;
            }
            else
                request.AgentId = null;

        }

    }
}
