using System;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AniGoldShop.Application.Common
{
    public class BaseController : Controller
    {
        public string ImagePathDirectory => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        public string ManageAreaViewRouting => "/Areas/Manage/Views/";

   
        //protected new virtual LoginModel CurrentUser
        //{
        //    get
        //    {
        //        var model = new LoginModel();

        //        try
        //        {
        //            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
        //            {
        //                var json = HttpContext.User.FindFirst(ClaimTypes.UserData);
        //                if (json != null)
        //                {
        //                    var jsonValue = json.Value;
        //                    model = JsonConvert.DeserializeObject<LoginModel>(jsonValue);
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return new LoginModel();
        //        }
        //        return model;
        //    }
        //}

    }
}
