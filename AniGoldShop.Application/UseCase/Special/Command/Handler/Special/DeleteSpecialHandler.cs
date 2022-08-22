using AniGoldShop.Application.Common.Localization.Text;
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
    public class DeleteSpecialHandler : IRequestHandler<DeleteSpecialRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Specials, Guid> _repository;
        private IRepository<Domain.Entities.Blogs, Guid> _blogRepository;

        public DeleteSpecialHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Specials, Guid> repository,
            IRepository<Domain.Entities.Blogs, Guid> blogRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _blogRepository = blogRepository;

        }
        public async Task<FuncResult> Handle(DeleteSpecialRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {

                await _repository.Delete(w=> w.ProductId== request.ProductId.Value && w.SpecialType==request.Type, true);
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
