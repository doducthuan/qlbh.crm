using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using QLBH_Dion.Repository;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util;
using QLBH_Dion.Constants;
using QLBH_Dion.Models;

namespace QLBH_Dion.Helper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly int[] roleIds;
        public CustomAuthorizeAttribute(params int[] _roleIds)
        {
            roleIds = _roleIds;
        }
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var url = context.HttpContext.Request.Path.Value;
            try
            {
                //var tokenService = context.HttpContext.RequestServices.GetService<ITokenService>();
                var accountService = context.HttpContext.RequestServices.GetService<IAccountRepository>();
                var tokenString = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                //if (url.Contains("api"))
                //{
                //    var isTokenValid = tokenService.ValidateToken(tokenString);
                //    if (isTokenValid)
                //    {
                //        var token = tokenService.ParseToken(tokenString);
                //        var userId = Convert.ToInt32(token.Claims.First(c => c.Type == ClaimNames.ID).Value);

                //        var account = await accountService.Detail(userId);

                //        if (!roleIds.Contains(account[0].RoleId))
                //        {
                //            context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                //        }
                //    }
                //    else
                //    {
                //        context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                //    }
                //}
                if (!url.Contains("api"))
                {
                    var accountId = context.HttpContext.Session.GetInt32("UserId");
                    if (accountId == null)
                    {
                        var result = new RedirectToActionResult("SignIn", "Home", null);
                        context.Result = result;
                    }
                    else
                    {
                        var account = await accountService.Detail(accountId);
                        if (!roleIds.Contains(account[0].RoleId))
                        {
                            var result = new PartialViewResult
                            {
                                ViewName = "Error403"
                            };
                            context.Result = result;
                        }
                    }

                }

            }
            catch
            {
                if (url.Contains("api"))
                {
                    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                }
                else
                {
                    var result = new PartialViewResult
                    {
                        ViewName = "Error403"
                    };
                    context.Result = result;
                }

            }
        }
    }
}
