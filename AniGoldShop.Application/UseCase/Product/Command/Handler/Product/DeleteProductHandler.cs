 using AniGoldShop.Application.Common.Localization.Text;
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
    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Products, Guid> _repository;
        private IRepository<Domain.Entities.Specials, Guid> _specRepository;
        private IRepository<Domain.Entities.FactorItems, Guid> _FactorItemrepository;

        public DeleteProductHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Products, Guid> repository,
            IRepository<Domain.Entities.Specials, Guid> specRepository,
            IRepository<Domain.Entities.FactorItems, Guid> FactorItemrepository
            )
        {
            _localize = localize;
            _repository = repository;
            _specRepository = specRepository;
            _FactorItemrepository = FactorItemrepository;

        }
        public async Task<FuncResult> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                if (request.ForceDelete)
                {
                    await _specRepository.Delete(w => w.ProductId == request.Id);
                    await _FactorItemrepository.Delete(w=> w.ProductId == request.Id);
                }

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
