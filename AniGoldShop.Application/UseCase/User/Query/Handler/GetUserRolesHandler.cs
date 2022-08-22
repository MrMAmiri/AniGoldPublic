using AniGoldShop.Application.Common.Localization.Text;
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
using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.UseCase.User.Query.Request;

namespace AniGoldShop.Application.UseCase.User.Query.Handler
{
    public class GetUserRolesHandler : IRequestHandler<GetUserRolesRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.UserRoles, Guid> _repository;
        private IRepository<Domain.Entities.Roles, Guid> _roleRepository;
        private IRepository<Domain.Entities.Users, Guid> _userRepository;

        public GetUserRolesHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.UserRoles, Guid> repository,
            IRepository<Domain.Entities.Roles, Guid> roleRepository,
            IRepository<Domain.Entities.Users, Guid> userRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;

        }
        public async Task<FuncResult> Handle(GetUserRolesRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                if (!request.OnlyUsers) {
                    var resCount = await _repository.CountGODAsync(w =>
                    (request.Name == null || w.User.UserFullName.Contains(request.Name) || w.User.UserUsername.Contains(request.Name))
                    &&
                    (request.RoleId == null || w.RoleId == request.RoleId)
                    &&
                    (request.Status == null || w.Status == request.Status)
                    , request.PageSize.Value, request.PageNumber.Value);


                    var res = await _repository.FindGODAsync(
                    w =>
                    (request.Name == null || w.User.UserFullName.Contains(request.Name) || w.User.UserUsername.Contains(request.Name))
                    &&
                    (request.RoleId == null || w.RoleId == request.RoleId)
                    &&
                    (request.Status == null || w.Status == request.Status)
                    , o => o.CreateDate, true
                        , request.PageNumber.Value, request.PageSize.Value
                        , i1 => i1.Role, i2 => i2.User);

                    if (res != null && res.Any())
                    {
                        funcresult.Data = new
                        {
                            data = res.Select(s => new
                            {
                                id = s.UserRoleId,
                                fullName = s.User.UserFullName,
                                userName = s.User.UserUsername,
                                roleName = s.Role.RoleName,
                                nationalId = s.User.UserNationalId,
                                userId = s.UserId,
                                roleId = s.RoleId,
                                status = s.Status,
                                statusName = s.Status <= 0 ? "غیرفعال" : "فعال",
                                createDate = s.CreateDate.ToPersianDateString()
                            }).ToList(),
                            total = resCount.Item1,
                            pageCount = resCount.Item2,
                            nextPage = resCount.Item3
                        };
                        funcresult.Message = "عملیات با موفقیت انجام شد";
                        funcresult.Successful = true;
                    }
                    else
                    {
                        funcresult.Message = "موردی یافت نشد";
                    }
                }
                else
                {
                    var resCount = await _userRepository.CountGODAsync(w =>
                   (request.Name == null || w.UserFullName.Contains(request.Name) || w.UserUsername.Contains(request.Name))
                   &&
                   (request.RoleId == null || w.UserRoles.Any(f=> f.RoleId== request.RoleId))
                   &&
                   (request.Status == null || w.Status == request.Status)
                   , request.PageSize.Value, request.PageNumber.Value);

                    var res = await _userRepository.FindGODAsync(
                    w =>
                   (request.Name == null || w.UserFullName.Contains(request.Name) || w.UserUsername.Contains(request.Name))
                   &&
                   (request.RoleId == null || w.UserRoles.Any(f => f.RoleId == request.RoleId))
                   &&
                   (request.Status == null || w.Status == request.Status)
                    , o => o.CreateDate, true
                        , request.PageNumber.Value, request.PageSize.Value
                        , i1 => i1.UserRoles);

                    if (res != null && res.Any())
                    {
                        funcresult.Data = new
                        {
                            data = res.Select(s => new
                            {
                                id = s.UserId,
                                fullName = s.UserFullName,
                                userName = s.UserUsername,
                                roleName = GetRoleName(s.UserRoles.FirstOrDefault().RoleId,cancellationToken),
                                roleId=s.UserRoles.FirstOrDefault().RoleId,
                                nationalId = s.UserNationalId,
                                cellPhone=s.UserCellPhone,
                                address=s.UserAddress,
                                status = s.Status,
                                statusName = s.Status <= 0 ? "غیرفعال" : "فعال",
                                createDate = s.CreateDate.ToPersianDateString()
                            }).ToList(),
                            total = resCount.Item1,
                            pageCount = resCount.Item2,
                            nextPage = resCount.Item3
                        };
                        funcresult.Message = "عملیات با موفقیت انجام شد";
                        funcresult.Successful = true;
                    }
                    else
                    {
                        funcresult.Message = "موردی یافت نشد";
                    }
                }
            }
            catch (Exception ex)
            {
                funcresult.Message = ex.Message;
            }

            return funcresult;
        }

        public string GetRoleName(Guid? roleId, CancellationToken cancellationToken)
        {
            return _roleRepository.Find(s => s.RoleId == roleId).FirstOrDefault().RoleName;
        }

    }
}
