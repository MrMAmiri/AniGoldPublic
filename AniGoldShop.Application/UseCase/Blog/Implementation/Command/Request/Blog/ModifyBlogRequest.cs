using AniGoldShop.Application;
using AniGoldShop.Application.Common;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AniGoldShop.Application.UseCase.Blog.Command.Request
{
    public class ModifyBlogRequest : BaseRequest, IRequest<FuncResult>
    {
        public Guid? Id { get; set; }
        public Guid? GroupId { get; set; }
        public string Summary { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tags { get; set; }
        public Guid? AuthorId { get; set; }
        public string[] Images { get; set; }
        
    }
}
