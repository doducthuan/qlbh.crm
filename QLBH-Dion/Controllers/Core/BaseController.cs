using QLBH_Dion.Helper;
using QLBH_Dion.Models;
using QLBH_Dion.Services;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using QLBH_Dion.Models;
using QLBH_Dion.Util;

namespace QLBH_Dion.Controllers.Core
{
    public class BaseController : Controller
    {
        public static string SystemURL = "";

        //public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        //{
        //    string ServerURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/";
        //    SystemURL = ServerURL;
        //    ViewBag.SystemURL = ServerURL;
        //    string RequestedURL = filterContext.HttpContext.Request.Path.ToString().ToLower();

        //    if (!RequestedURL.Contains("api") && !RequestedURL.Contains("sign-in"))
        //    {
        //        var accountId = filterContext.HttpContext.Session.GetInt32("UserId");
        //        if (accountId == null)
        //        {
        //            filterContext.Result = RedirectToAction("signin", "home");
        //        }
        //        else
        //        {
        //            var userService = filterContext.HttpContext.RequestServices.GetService<IAccountService>();
        //            var accounts = await userService.Detail(accountId);
        //            if (accounts == null)
        //            {
        //                filterContext.Result = RedirectToAction("signin", "home");

        //            }
        //            else
        //            {
        //                ViewBag.RoleId = accounts[0].RoleId;
        //                ViewBag.AccountId = accounts[0].Id;

        //            }
        //        }
        //    }
        //    if (RequestedURL == "/")
        //    {
        //        filterContext.Result = RedirectToAction("adminlistserverside", "orders");
        //    }
        //    SystemConstant.DEFAULT_URL = ServerURL;
        //    ViewBag.ServerURL = ServerURL;

        //    await base.OnActionExecutionAsync(filterContext, next);

        //}

        //public override async void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    string ServerURL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/";
        //    SystemURL = ServerURL;
        //    ViewBag.SystemURL = ServerURL;
        //    string RequestedURL = filterContext.HttpContext.Request.Path.ToString().ToLower();
        //    //VALIDATE REQUEST
        //    //int AccountId = this.GetLoggedInUserId();
        //    //int RoleId = this.GetLoggedInRoleId();

        //    if (!RequestedURL.Contains("api") && !RequestedURL.Contains("sign-in"))
        //    {
        //        var accountId = HttpContext.Session.GetInt32("UserId");
        //        if (accountId == null)
        //        {
        //            filterContext.Result = RedirectToAction("signin", "home");
        //        }
        //        ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
        //    }
        //    //else
        //    //{
        //    //    if (RequestedURL.Contains("api") && !RequestedURL.Contains("api/login"))
        //    //    {
        //    //        var accountIdFromToken = this.GetLoggedInUserId();
        //    //        if (accountIdFromToken == 0)
        //    //        {
        //    //            filterContext.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
        //    //        }

        //    //    }
        //    //}

        //    //Điều hướng sang 401 nếu không valid
        //    //var systemConfigs = _cacheHelper.GetSystemConfig();
        //    //ViewBag.SystemConfigs = systemConfigs;
        //    //Lấy server URL động
        //    //ViewBag.SystemConfigs["HOMEPAGE_URL"].Description = ServerURL;
        //    SystemConstant.DEFAULT_URL = ServerURL;
        //    ViewBag.ServerURL = ServerURL;
        //}
    }
}
