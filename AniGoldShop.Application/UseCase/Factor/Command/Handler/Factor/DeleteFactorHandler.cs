using AniGoldShop.Application.Common.Localization.Text;
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
    public class DeleteFactorHandler : IRequestHandler<DeleteFactorRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Factors, Guid> _repository;
        private IRepository<Domain.Entities.FactorItems, Guid> _itemRepository;
        private IRepository<Domain.Entities.UserRoles, Guid> _userRoleRepo;


        public DeleteFactorHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Factors, Guid> repository,
            IRepository<Domain.Entities.FactorItems, Guid> itemRepository,
            IRepository<Domain.Entities.UserRoles, Guid> userRoleRepo
            )
        {
            _localize = localize;
            _repository = repository;
            _itemRepository = itemRepository;
            _userRoleRepo = userRoleRepo;

        }
        public async Task<FuncResult> Handle(DeleteFactorRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                await CheckPerms(request, cancellationToken);

                if (request.DeleteByStatus != null && request.DeleteByStatus.Value)
                {
                    var fc =await _repository.Find(request.Id.Value);

                    if(fc!=null)
                    {
                        if (request.IsCustomer)
                            if (fc.UserId == request.ReqUserId)
                            {
                                fc.Status = 100;
                                fc.ModifiedUser = request.Modifier;
                                fc.ModifiedDate = DateTime.Now;
                                await _repository.SaveChange();
                            }
                    }
                }
                else
                {
                    await _itemRepository.Delete(w => w.FactorId == request.Id);
                    await _repository.Delete(request.Id.Value, true);
                }

                funcresult.Message = "عملیات با موفقیت انجام شد";
                funcresult.Successful = true;
            }
            catch (Exception ex)
            {
                funcresult.Message = "خطا در حذف اطلاعات";
            }

            return funcresult;
        }

        async Task CheckPerms(DeleteFactorRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var rr = (await _userRoleRepo.FindAsync(w => w.UserRoleId == request.Modifier));

                if (rr != null)
                {
                    if (rr.Any(w => w.RoleId == Guid.Parse("84A8B08C-8BC0-4DFF-B5B8-6E60BA96A633")))
                    {
                        request.ReqUserId = rr.FirstOrDefault().UserId;
                        request.IsCustomer = true;
                        request.DeleteByStatus = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }

    }
}
