

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

namespace QLBH_Dion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountBuyController : BaseController
    {
        IAccountBuyService service;
        public AccountBuyController(IAccountBuyService _service, ICacheHelper cacheHelper)
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
        public async Task<IActionResult> Add([FromBody] AccountBuy model)
        {
            if (ModelState.IsValid)
            {
                //1. business logic

                ////data validation
                //if (model.Name.Length == 0)
                //{
                //    return BadRequest();
                //}
                //2. add new object
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
        public async Task<IActionResult> Update([FromBody] AccountBuy model)
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
        public async Task<IActionResult> Delete([FromBody] AccountBuy model)
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
        public async Task<IActionResult> DeletePermanently([FromBody] AccountBuy model)
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
        public int CountAccountBuy()
        {
            int result = service.Count();
            return result;
        }
        [HttpPost]
        [Route("api/list-server-side")]
        public async Task<IActionResult> ListServerSide([FromBody] AccountBuyDTParameters parameters)
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
        [Route("api/accountbuy-paging")]
        public async Task<IActionResult> ListAccountBuyByOrderId([FromBody] PagingModel obj)
        {
            try
            {
                var data = await service.ListAccountBuyByOrderId(obj);
                return Ok(data);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/Detail2/{Id}")]
        public async Task<IActionResult> Detail2(int Id)
        {
            try
            {
                var data = await service.Detail2(Id);
                return Ok(data);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("api/lock-account")]
        public async Task<IActionResult> LockAccountBuy(AccountBuy accountBuy)
        {
            try
            {
                await service.LockAccountBuy(accountBuy);
                return Ok(accountBuy);
            }catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}
