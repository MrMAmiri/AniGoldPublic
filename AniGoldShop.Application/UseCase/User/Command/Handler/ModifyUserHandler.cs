using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.User.Command.FluentValidation;
using AniGoldShop.Application.UseCase.User.Command.Request;
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

namespace AniGoldShop.Application.UseCase.User.Command.Handler
{
    public class ModifyUserHandler : IRequestHandler<ModifyUserRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Users, Guid> _repository;
        private IRepository<Domain.Entities.UserRoles, Guid> _uroleRepository;

        public ModifyUserHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Users, Guid> repository,
            IRepository<Domain.Entities.UserRoles, Guid> uroleRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _uroleRepository = uroleRepository;

        }
        public async Task<FuncResult> Handle(ModifyUserRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                var validator = new UserFluentValidation(_localize, _repository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }

                Domain.Entities.Users ent = null;
                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.UserFullName = request.FullName;
                    ent.UserUsername = request.UserName;
                    ent.UserPassword =(!string.IsNullOrWhiteSpace( request.Password))? SecurityHelper.GetHash(request.Password):ent.UserPassword;
                    ent.UserCellPhone = request.CellPhone;
                    ent.UserAddress = request.Address;
                    ent.UserNationalId=request.NationalId;
                    ent.Status = request.Status;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;

                    var urole = (await _uroleRepository.FindAsync(w => w.UserId == request.Id)).FirstOrDefault();

                    if (request.RoleId!=null && urole == null)
                    {
                        urole = new Domain.Entities.UserRoles()
                        {
                            CreateDate = DateTime.Now,
                            RoleId = request.RoleId.Value,
                            UserId = request.Id.Value,
                            CreateUser = request.Modifier,
                            Status = 1,
                            UserRoleId = Guid.NewGuid()
                        };

                        await _uroleRepository.Insert(urole);
                    }
                    else if(request.RoleId!=null && urole != null)
                    {
                        urole.RoleId = request.RoleId.Value;
                        urole.ModifiedUser = request.Modifier;
                        urole.ModifiedDate= DateTime.Now;
                        await _uroleRepository.Update(urole);
                    }

                    await _repository.Update(ent, true);
                }
                else
                {
                    ent = new Domain.Entities.Users()
                    {
                        UserFullName = request.FullName,
                        UserUsername = request.UserName,
                        UserPassword = SecurityHelper.GetHash(request.Password),
                        UserCellPhone = request.CellPhone,
                        UserAddress = request.Address,
                        UserNationalId=request.NationalId,
                        ModifiedUser = request.Modifier,
                        Status = request.Status,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        UserId = Guid.NewGuid()
                    };
                    await _repository.Insert(ent);

                    await _uroleRepository.Insert(new Domain.Entities.UserRoles()
                    {
                        RoleId = request.RoleId.Value,
                        UserId = ent.UserId,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        Status = 1,
                        UserRoleId = Guid.NewGuid()
                    },true);
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
