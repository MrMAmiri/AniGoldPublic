using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Special.Command.FluentValidation;
using AniGoldShop.Application.UseCase.Special.Command.Request;
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

namespace AniGoldShop.Application.UseCase.Special.Command.Handler
{
    public class ModifySpecialHandler : IRequestHandler<ModifySpecialRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Specials, Guid> _repository;

        public ModifySpecialHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Specials, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(ModifySpecialRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                var validator = new SpecialFluentValidation(_localize, _repository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }

                Domain.Entities.Specials ent = null;
                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.ProductId = request.ProductId;
                    ent.SpecialStart = request.Start.ToGeorgDate();
                    ent.SpecialEnd = request.End.ToGeorgDate();
                    ent.SpecialType = request.Type;
                    ent.Status = request.Status;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;
                    await _repository.Update(ent, true);
                }
                else
                {
                    ent = new Domain.Entities.Specials()
                    {
                        ProductId = request.ProductId,
                        SpecialStart = request.Start.ToGeorgDate(),
                        SpecialEnd = request.End.ToGeorgDate(),
                        SpecialType = request.Type,
                        ModifiedUser = request.Modifier,
                        Status = request.Status,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        SpecialId = Guid.NewGuid()
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
