using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.FileUpload.Command.Request
{
    public class DeleteFileRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
    }
}
