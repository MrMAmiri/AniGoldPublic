using AniGoldShop.Application.Common.Helper;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Application.UseCase.Blog.Command.FluentValidation;
using AniGoldShop.Application.UseCase.Blog.Command.Request;
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

namespace AniGoldShop.Application.UseCase.Blog.Command.Handler
{
    public class ModifyBlogHandler : IRequestHandler<ModifyBlogRequest, FuncResult>
    {
        private IStringLocalizer<TextLocalizationResource> _localize;
        private IRepository<Domain.Entities.Blogs, Guid> _repository;
        private IRepository<Domain.Entities.UserRoles, Guid> _uroleRepository;

        public ModifyBlogHandler(
            IStringLocalizer<TextLocalizationResource> localize,
            IRepository<Domain.Entities.Blogs, Guid> repository,
            IRepository<Domain.Entities.UserRoles, Guid> uroleRepository
            )
        {
            _localize = localize;
            _repository = repository;
            _uroleRepository = uroleRepository;

        }
        public async Task<FuncResult> Handle(ModifyBlogRequest request, CancellationToken cancellationToken)
        {
            FuncResult funcresult = new FuncResult();
            try
            {
                var validator = new BlogFluentValidation(_localize, _repository);
                var result = await validator.ValidateAsync(request, cancellationToken);
                if (!result.IsValid)
                {
                    var errMsg = string.Join(Environment.NewLine, result.Errors.Select(s => s.ErrorMessage.ToString()).ToList());
                    funcresult.Message = errMsg;
                    return funcresult;
                }


                var user = (await _uroleRepository.FindAsync(w => w.UserRoleId == request.Modifier.Value))
                    .FirstOrDefault();

                Domain.Entities.Blogs ent = null;
                if (request.Id != null)
                {
                    ent = await _repository.Find(request.Id.Value);
                    ent.BlogSummary = request.Summary;
                    ent.BlogTitle = request.Title;
                    ent.BlogImages = request.Images.CJoin();
                    ent.BlogTags = request.Tags;
                    ent.BlogText = request.Text;
                    ent.BlogAuthor = user.UserId;
                    ent.BlogGroupId = request.GroupId;
                    ent.Status = request.Status;
                    ent.ModifiedDate = DateTime.Now;
                    ent.ModifiedUser = request.Modifier;
                    await _repository.Update(ent, true);
                }
                else
                {
                    ent = new Domain.Entities.Blogs()
                    {
                        BlogSummary = request.Summary,
                        BlogTitle = request.Title,
                        BlogImages = request.Images.CJoin(),
                        BlogTags = request.Tags,
                        BlogText = request.Text,
                        BlogAuthor = user.UserId,
                        BlogGroupId = request.GroupId,
                        ModifiedUser = request.Modifier,
                        Status = request.Status,
                        ModifiedDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUser = request.Modifier,
                        BlogId = Guid.NewGuid()
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
