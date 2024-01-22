

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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using QLBH_Dion.Helper;
using QLBH_Dion.Constants;
using FirebaseNet.Database;

namespace QLBH_Dion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationController : BaseController
    {
        INotificationService service;
        IAccountRepository repositoryAccount;
        public NotificationController(INotificationService _service, ICacheHelper cacheHelper, IAccountRepository _repositoryAccount)
        {
            service = _service;
            repositoryAccount = _repositoryAccount;
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
        public async Task<IActionResult> Add([FromBody] Notification model)
        {
            if (ModelState.IsValid)
            {
                //1. business logic

                //data validation
                if (model.Name.Length == 0)
                {
                    return BadRequest();
                }
                //2. add new object
                try
                {
                    await service.Add(model);
                    var QLBH_DionResponse = QLBH_Response.CREATED(model);
                    return Created("", QLBH_DionResponse);
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
        public async Task<IActionResult> Update([FromBody] Notification model)
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
        public async Task<IActionResult> Delete([FromBody] Notification model)
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
        public async Task<IActionResult> DeletePermanently([FromBody] Notification model)
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
        public int CountNotification()
        {
            int result = service.Count();
            return result;
        }

        [HttpPost]
        [Route("api/set-device")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SetDeviceFireBase([FromBody] FCMToken deviceToken)
        {
            try
            {
                var accountId = this.GetLoggedInUserId();
                var accountData = await repositoryAccount.Detail(accountId);
                if (accountData.Count == 0) return NotFound();
                if(deviceToken.Device == DeviceId.DEVICEWEB)
                {
                    accountData[0].GuidId = deviceToken.DeviceToken;
                }
                else
                {
                    accountData[0].GuidIdApp = deviceToken.DeviceToken;
                }
                var response = await repositoryAccount.SetDevice(accountData[0]);
                if (!response) return BadRequest();
                
                return Ok(QLBH_Response.SUCCESS(accountData));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }
        }
        [HttpPost]
        [Route("api/list-server-side")]
        public async Task<IActionResult> ListServerSide([FromBody] NotificationDTParameters parameters)
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
        [HttpPost]
        [Route("api/test-push-noti")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> TestPushNoti()
        {
            try
            {
                var deviceToken = "eeDJg5fGgDvBWUwPRmo1gT:APA91bHd06t8FXVi5jUMIpA2OlJ9nAXiRPEyBtVGs3A38CMJorhOhYsX2WUDcPzOlq2muwKLEWO5PPobQHE61blEJkKsz-KuGGPwd0HjsWJOofus0VQtZZNLlICFGmiw3L3_eVzApbtC";

                FCMExtension.SendNotificationToMobile(deviceToken, "Yêu cầu được cập nhật", "Yêu cầu 1099 đã bị hủy", NotificationKey.KEY_GET_NOTIFICATION, 1017);


                return Ok(QLBH_Response.SUCCESS());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " - " + ex.InnerException);
            }
        }
    }
}
