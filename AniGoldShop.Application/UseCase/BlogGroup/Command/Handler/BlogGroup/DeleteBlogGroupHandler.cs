using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.BlogGroup.Command.Request;
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

namespace AniGoldShop.Application.UseCase.BlogGroup.Command.Handler
{
    public class DeleteBlogGroupHandler : IRequestHandler<DeleteBlogGroupRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.BlogGroups, Guid> _repository;
        private IRepository<Domain.Entities.Blogs, Guid> _blogRepository;

        public DeleteBlogGroupHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.BlogGroups, Guid> repository,
            IRepository<Domain.Entities.Blogs, Guid> blogRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _blogRepository = blogRepository;

        }
        public async Task<FuncResult> Handle(DeleteBlogGroupRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                if(request.FourceDelete!=null && request.FourceDelete.Value)
                {
                    var blogs = (await _blogRepository.FindAsync(w => w.BlogGroupId == request.Id))
                        .ToList();
                    foreach(var item in blogs)
                    {
                        await _blogRepository.Delete(item.BlogId);
                    }
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
