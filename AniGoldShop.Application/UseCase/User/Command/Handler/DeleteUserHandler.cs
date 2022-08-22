
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
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.User.Command.Request;

namespace AniGoldShop.Application.UseCase.User.Command.Handler
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Users, Guid> _repository;
        private IRepository<Domain.Entities.UserRoles, Guid> _uroleRepository;

        public DeleteUserHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Users, Guid> repository,
            IRepository<Domain.Entities.UserRoles, Guid> uroleRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _uroleRepository = uroleRepository;

        }
        public async Task<FuncResult> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                await _uroleRepository.Delete(f=> f.UserId==request.Id.Value, true);
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
