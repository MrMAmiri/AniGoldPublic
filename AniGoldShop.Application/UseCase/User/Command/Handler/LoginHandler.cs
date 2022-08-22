using AniGoldShop.Application;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Domain.Interfaces;
using AniGoldShop.Application.UseCase.User.Command.Request;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AniGoldShop.Application.Common.Helper;

namespace AniGoldShop.Application.UseCase.User.Command.Handler
{
    public class LoginHandler : IRequestHandler<LoginRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Users, Guid> _repository;
        private IRepository<Domain.Entities.Roles, Guid> _roleRepository;
        private IRepository<Domain.Entities.UserRoles, Guid> _userRoleRepository;

        public LoginHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Users, Guid> userRepository,
            IRepository<Domain.Entities.UserRoles, Guid> userRoleRepository,
            IRepository<Domain.Entities.Roles, Guid> roleRepository)
        {
            _localize = localize;
            _repository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<FuncResult> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            FuncResult result = new FuncResult();

            try
            {
                //var validator = new LoginFluentValidation(_localize, _repository);
                //var result = await validator.ValidateAsync(request, cancellationToken);

                //if (!result.IsValid)
                //{
                //    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                //    return new LoginDTO
                //    {
                //        Message = errMsg,
                //        Status = Common.Enum.ResponseStatus.ValidationError
                //    };
                //}

                var userRes = (await _repository.FindGODAsync(
                    s => s.UserUsername == request.Username.ToLower()
                    && s.UserPassword == SecurityHelper.GetHash(request.Password),
                    null,
                    false,
                    1,500,
                    i1=> i1.UserRoles
                    )).FirstOrDefault();

                if (userRes == null && request.CreateIfNot)
                {
                    var xx = (await _repository.FindAsync(w => w.UserUsername.ToLower() == request.Username.ToLower()))
                        .Any();

                    if (xx)
                    {
                        result.Message = "رمز عبور اشتباه است";
                        return result;
                    }


                    var userId = Guid.NewGuid();
                    var user = new Domain.Entities.Users()
                    {
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Status = 1,
                        UserAddress = request.Address,
                        UserCellPhone = request.CellPhone,
                        UserEmail = request.Email,
                        UserFullName = request.Name,
                        UserId = userId,
                        UserPassword = SecurityHelper.GetHash(request.Password),
                        UserUsername = request.CellPhone
                    };

                    var userRoleId = Guid.NewGuid();
                    var usrole = new Domain.Entities.UserRoles()
                    {
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        RoleId = Guid.Parse("84A8B08C-8BC0-4DFF-B5B8-6E60BA96A633"),
                        Status = 1,
                        UserId = userId,
                        UserRoleId = userRoleId
                    };

                    await _repository.Insert(user);
                    await _userRoleRepository.Insert(usrole,true);

                    userRes = user;
                }


                if (userRes != null)
                {
                    if (userRes.Status == 0)
                    {
                        result.Message = "کاربری شما غیر فعال می باشد";
                        return result;
                    }

                    Domain.Entities.Roles role = null;
                    if (userRes.UserRoles != null && userRes.UserRoles.Any())
                        role = (await _roleRepository.FindAsync(s => s.RoleId == userRes.UserRoles.FirstOrDefault().RoleId)).FirstOrDefault();


                    var token = JwtHelper.generateJwtToken(
                          request.jwtSettings,
                            userRes.UserRoles.FirstOrDefault().UserRoleId,
                            userRes.UserFullName,
                            "fa-ir",
                            role.RoleName
                        );


                    result.Successful = true;
                    result.Data = new
                    {
                        userName = userRes.UserUsername,
                        userFullName = userRes.UserFullName,
                        userCellPhone=userRes.UserCellPhone,
                        userAddress=userRes.UserAddress,
                        userEmail=userRes.UserEmail,
                        role = role?.RoleName,
                        userId=userRes.UserId,
                        roleId=userRes.UserRoles?.FirstOrDefault()?.RoleId,
                        token=token
                    };
                    result.Message = "ورود موفقیت آمیز بود";
                }
                else
                {
                    result.Message = "نام کاربری یا رمز عبور اشتباه می باشد";
                }
            }
            catch (Exception ex)
            {
                result.Message = "خطای سیستمی، لطفا فیلد هارا تکمیب کنید";
            }

            return result;
        }
    }
}
