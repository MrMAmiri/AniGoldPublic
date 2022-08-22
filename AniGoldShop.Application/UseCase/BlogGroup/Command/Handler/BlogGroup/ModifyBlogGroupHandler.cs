using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.BlogGroup.Command.FluentValidation;
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
    public class ModifyBlogGroupHandler : IRequestHandler<ModifyBlogGroupRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.BlogGroups, Guid> _repository;

        public ModifyBlogGroupHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.BlogGroups, Guid> repository
            )
        {
            _localize = localize;
            _repository = repository;

        }
        public async Task<FuncResult> Handle(ModifyBlogGroupRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                var validator = new BlogGroupFluentValidation(_localize, _repository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }

                Domain.Entities.BlogGroups ent = null;
                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.BlogGroupName = request.Name;
                    ent.BlogGroupTitle = request.Title;
                    ent.BlogGroupIcon = request.Icon;
                    ent.Status = request.Status;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;
                    await _repository.Update(ent, true);
                }
                else
                {
                    ent = new Domain.Entities.BlogGroups()
                    {
                        BlogGroupName = request.Name,
                        BlogGroupTitle= request.Title,
                        BlogGroupIcon= request.Icon,
                        ModifiedUser = request.Modifier,
                        Status = request.Status,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        BlogGroupId = Guid.NewGuid()
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
