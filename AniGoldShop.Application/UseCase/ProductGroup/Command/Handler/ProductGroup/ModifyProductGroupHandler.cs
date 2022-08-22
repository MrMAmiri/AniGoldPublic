using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.ProductGroup.Command.FluentValidation;
using AniGoldShop.Application.UseCase.ProductGroup.Command.Request;
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

namespace AniGoldShop.Application.UseCase.ProductGroup.Command.Handler
{
    public class ModifyProductGroupHandler : IRequestHandler<ModifyProductGroupRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.ProductGroups, Guid> _repository;

        public ModifyProductGroupHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.ProductGroups, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(ModifyProductGroupRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                var validator = new ProductGroupFluentValidation(_localize, _repository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }

                Domain.Entities.ProductGroups ent = null;
                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.ProductGroupName = request.Name;
                    ent.ProductGroupTitle = request.Title;
                    ent.ProductGroupIcon = request.Icon;
                    ent.Status = request.Status;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;
                    ent.Priority= request.Priority;
                    await _repository.Update(ent, true);
                }
                else
                {
                    ent = new Domain.Entities.ProductGroups()
                    {
                        ProductGroupName = request.Name,
                        ProductGroupTitle= request.Title,
                        ProductGroupIcon= request.Icon,
                        ModifiedUser = request.Modifier,
                        Status = request.Status,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        Priority=request.Priority,
                        ProductGroupId = Guid.NewGuid()
                    };
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
