

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Models;
using QLBH_Dion.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QLBH_Dion.Util;
using QLBH_Dion.Controllers.Core;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Constants;
using QLBH_Dion.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using QLBH_Dion.Helper;
using QLBH_Dion.Models.ViewModels;

namespace QLBH_Dion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        IAccountService service;
        ILogger<AccountController> logger;
        public AccountController(IAccountService _service, ICacheHelper cacheHelper)
        {
            service = _service;
        }
        [HttpGet]
        [Route("admin/List")]
        public async Task<IActionResult> AdminListServerSide()
        {
            try
            {
                var dataList = await service.List();
                if (dataList == null || dataList.Count == 0)
                {
                    return View(dataList);
                }
                ViewBag.entities = dataList;
                return View(dataList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/List")]
        public async Task<IActionResult> List()
        {
            try
            {
                var dataList = await service.List();
                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }
                var QLBH_DionResponse = QLBH_Response.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(QLBH_DionResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/Detail/{Id}")]
        public async Task<IActionResult> Detail(int? Id)
        {
            if (Id == null)
            {
                return BadRequest();
            }
            try
            {
                var dataList = await service.Detail(Id);
                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }
                var QLBH_DionResponse = QLBH_Response.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(QLBH_DionResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/Detail2/{Id}")]
        public async Task<IActionResult> Detail2(int? Id)
        {
            if (Id == null)
            {
                return BadRequest();
            }
            try
            {
                var data = await service.Detail2(Id.Value);
                if (data == null)
                {
                    return NotFound();
                }
                var QLBH_DionResponse = QLBH_Response.SUCCESS(data);
                return Ok(QLBH_DionResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var data = await service.Login(model);
                var repsonse = QLBH_Response.Success(data);
                if (data != null)
                {
                    if (data.AccountStatusId == AccountStatusId.LOCKED)
                    {
                        repsonse = QLBH_Response.BlockedAccount();
                    }
                }
                else
                {
                    repsonse = QLBH_Response.WrongAccount();
                }
                return Ok(repsonse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("admin/info")]
        public IActionResult InfoProfile(int roleId)
        {
            ViewBag.RoleId = roleId;
            return View();
        }
        [HttpGet]
        [Route("api/Search")]
        public async Task<IActionResult> Search(string keyword)
        {
            try
            {
                var dataList = await service.Search(keyword);
                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }
                var QLBH_DionResponse = QLBH_Response.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(QLBH_DionResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("api/ListPaging")]
        public async Task<IActionResult> ListPaging(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0) return BadRequest();
            try
            {
                var dataList = await service.ListPaging(pageIndex, pageSize);

                if (dataList == null || dataList.Count == 0)
                {
                    return NotFound();
                }

                var QLBH_DionResponse = QLBH_Response.SUCCESS(dataList.Cast<object>().ToList());
                return Ok(QLBH_DionResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("api/Add")]
        public async Task<IActionResult> Add([FromBody] Account model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool check = await service.CheckExits(model.Username);
                    if (!check)
                    {
                        await service.Add(model);
                        var QLBH_DionResponse = QLBH_Response.CREATED(model);
                        return Created("", QLBH_DionResponse);
                    }
                    return BadRequest();
                    
                }
                catch (Exception)
                {

                    return BadRequest();
                }
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("api/Update")]
        public async Task<IActionResult> Update([FromBody] Account model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //1. business logic 
                    //2. update object
                    await service.Update(model);
                    var QLBH_DionResponse = QLBH_Response.SUCCESS(model);
                    return Ok(QLBH_DionResponse);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("api/Delete")]
        public async Task<IActionResult> Delete([FromBody] Account model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //1. business logic
                    await service.Delete(model);
                    var QLBH_DionResponse = QLBH_Response.SUCCESS(model);
                    return Ok(QLBH_DionResponse);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("api/DeletePermanently")]
        public async Task<IActionResult> DeletePermanently([FromBody] Account model)
        {
            var result = 0;
            if (!(model.Id > 0))
            {
                return BadRequest();
            }
            try
            {
                //physically delete object
                result = await service.DeletePermanently(model.Id);
                if (result == 0)
                {
                    return NotFound();
                }
                var QLBH_DionResponse = QLBH_Response.SUCCESS(model);
                return Ok(QLBH_DionResponse);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("api/Count")]
        public int CountAccount()
        {
            int result = service.Count();
            return result;
        }
        [HttpPost]
        [Route("api/list-server-side")]
        public async Task<IActionResult> ListServerSide([FromBody] AccountDTParameters parameters)
        {
            try
            {
                var data = await service.ListServerSide(parameters);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet]
        [Route("api/get-info-account")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetInfoAccountById()
        {
            try
            {
                var accountId = this.GetLoggedInUserId();
                var account = await service.GetInfoAccountById(accountId);
                if (account == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(QLBH_Response.Success(account));
                }

            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("api/update-fullname")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateFullNameById([FromBody] AccountViewModel model)
        {
            try
            {
                if (model.FullName.Length == 0)
                {
                    return BadRequest();
                }
                else
                {
                    var accountId = this.GetLoggedInUserId();
                    var result = await service.UpdateFullNameById(accountId, model.FullName);
                    if (result)
                    {
                        return Ok(QLBH_Response.Success(model));
                    }
                    return BadRequest();
                }


            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPut]
        [Route("api/update-password")]
        public async Task<IActionResult> UpdatePasswordById([FromBody] ChangePassword model)
        {
            try
            {
                if ((model.OldPassword.Length < 6) || (model.NewPassword.Length < 6) || (model.Confirm.Length < 6))
                {
                    return BadRequest();
                }
                else
                {
                    var accountId = HttpContext.Session.GetInt32("UserId");
                    var result = await service.UpdatePasswordById(Convert.ToInt32(accountId), model);
                    if (result)
                    {
                        return Ok(QLBH_Response.Success(model));
                    }
                    return BadRequest();
                }
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("api/update-by-view-model")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateByViewModel([FromBody] UpdateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<string> listErrors = new();

                    //check user name exist
                    if (await service.IsUserNameExist(model.Username, model.Id))
                    {
                        listErrors.Add("Tên ??ng nh?p ?ã ???c s? d?ng");
                    }

                    if (listErrors.Count > 0)
                    {
                        var response = QLBH_Response.BAD_REQUEST(listErrors);
                        return Ok(response);
                    }

                    if (await service.UpdateByViewModel(model))
                    {
                        var vatino_crmResponse = QLBH_Response.SUCCESS(model);
                        return Ok(vatino_crmResponse);
                    }
                    return BadRequest();

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"failed to update account by view model");

                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("api/lock-account")]
        public async Task<IActionResult> LockAccount([FromBody] Account account)
        {
            try
            {
                await service.LockAccount(account);
                return Ok(account);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}
