using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.FileUpload.Command.Request
{
    public class ModifyFileRequest : BaseRequest, IRequest<FuncResult>
    {
        public IFormFile[] file { get; set; }
        public string address { get; set; }
        public string type { get; set; }


    }
}
