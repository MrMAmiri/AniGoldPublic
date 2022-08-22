using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Slide.Query.Request
{
    public class GetSlidesRequest : BaseRequest, IRequest<FuncResult>
    {
        public short? Type { get; set; }
        public short? FileType { get; set; }
        public bool OnlyActives { get; set; } = false;
 
    }
}
