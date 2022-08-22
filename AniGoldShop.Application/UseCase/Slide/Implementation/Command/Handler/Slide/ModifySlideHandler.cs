using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Slide.Command.FluentValidation;
using AniGoldShop.Application.UseCase.Slide.Command.Request;
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

namespace AniGoldShop.Application.UseCase.Slide.Command.Handler
{
    public class ModifySlideHandler : IRequestHandler<ModifySlideRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Slides, Guid> _repository;
        private IRepository<Domain.Entities.UserRoles, Guid> _uroleRepository;

        public ModifySlideHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Slides, Guid> repository,
            IRepository<Domain.Entities.UserRoles, Guid> uroleRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _uroleRepository = uroleRepository;

        }
        public async Task<FuncResult> Handle(ModifySlideRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                var validator = new SlideFluentValidation(_localize, _repository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }


                var user = (await _uroleRepository.FindAsync(w => w.UserRoleId == request.Modifier.Value))
                    .FirstOrDefault();

                Domain.Entities.Slides ent = null;
                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.SlideType = request.Type;
                    ent.SlideFileType = request.FileType;
                    ent.SlideEnd = request.End.ToGeorgDate();
                    ent.SlideStart = request.Start.ToGeorgDate();
                    ent.SlideHtml = request.HTML;
                    ent.SlideLink = request.Link;
                    ent.SlideFile = request.File;
                    ent.Status = request.Status;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;
                    await _repository.Update(ent, true);
                }
                else
                {
                    ent = new Domain.Entities.Slides()
                    {
                        SlideType = request.Type,
                        SlideFileType = request.FileType,
                        SlideEnd = request.End.ToGeorgDate(),
                        SlideStart = request.Start.ToGeorgDate(),
                        SlideHtml = request.HTML,
                        SlideLink = request.Link,
                        SlideFile = request.File,
                        ModifiedUser = request.Modifier,
                        Status = request.Status,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        SlideId = Guid.NewGuid()
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
