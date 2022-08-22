using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Slide.Command.Request
{
    public class ModifySlideRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public short Type { get; set; }
        public short FileType { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Link { get; set; }
        public string HTML { get; set; }
        public string File { get; set; }
        
    }
}
