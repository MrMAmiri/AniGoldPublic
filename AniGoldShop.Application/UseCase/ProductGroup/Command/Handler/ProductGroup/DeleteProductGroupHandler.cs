using AniGoldShop.Application.Common.Localization.Text;
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
    public class DeleteProductGroupHandler : IRequestHandler<DeleteProductGroupRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.ProductGroups, Guid> _repository;

        public DeleteProductGroupHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.ProductGroups, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(DeleteProductGroupRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                await _repository.Delete(request.Id.Value, true);
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
