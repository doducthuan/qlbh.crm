

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
using QLBH_Dion.Models.ViewModel;
using QLBH_Dion.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography.Xml;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using iText.Layout.Element;
using System.Windows.Input;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using System.IO;
using NPOI.HPSF;

using QLBH_Dion.Helper;
using Newtonsoft.Json;

namespace QLBH_Dion.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : BaseController
    {
        IOrdersService service;
        IOrderStatusService orderStatusService;
        IWebHostEnvironment Environment;
        IAccountService accountService;
        public OrdersController(IOrdersService _service, ICacheHelper cacheHelper, IWebHostEnvironment _Environment, IAccountService _accountService, IOrderStatusService _orderStatusService)
        {
            service = _service;
            Environment = _Environment;
            accountService = _accountService;
            orderStatusService = _orderStatusService;
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
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Add([FromBody] Order model)
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
                    //model.AccountId = this.GetLoggedInUserId();
                    await service.Add(model);
                    try
                    {
                        var orderDetail = await service.Detail2(model.Id);
                        List<DeviceToken> listGuidIds = await accountService.GetDeviceToken();
                        foreach (DeviceToken guidIds in listGuidIds)
                        {
                            if (!string.IsNullOrEmpty(guidIds.GuidId))
                            {
                                FCMExtension.SendNotificationToMobile(guidIds.GuidId, "Yêu cầu mới","Yêu cầu #" + model.Id.ToString() + ": Biển số " + orderDetail.ProductName + " với giá " + String.Format("{0:0,0}", orderDetail.Price) + " VNĐ vừa được khởi tạo.", NotificationKey.KEY_GET_NOTIFICATION, model.Id);
                            }
                            if (!string.IsNullOrEmpty(guidIds.GuidIdApp))
                            {
                                FCMExtension.SendNotificationToMobile(guidIds.GuidIdApp, "Yêu cầu mới", "Yêu cầu #" + model.Id.ToString() + ": Biển số " + orderDetail.ProductName + " với giá " + String.Format("{0:0,0}", orderDetail.Price) + " VNĐ vừa được khởi tạo.", NotificationKey.KEY_GET_NOTIFICATION, model.Id);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        
                    }
                    
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
        public async Task<IActionResult> Update([FromBody] Order model)
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
        [Route("api/update-by-viewmodel")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateByViewModel([FromBody] Order model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //1. business logic 
                    //2. update object
                    
                    var orderBefor = await service.GetOrderStatusAndFinal(model.Id);
                    
                    int orderStatusIdUpdate = model.OrderStatusId;


                    var orderUpdateHistory = new OrderUpdateHistory();
                    bool confirmChange = false;
                    // set value orderUpdateHistory
                    if(model.Price != orderBefor.Price || model.FinalPrice != orderBefor.FinalPrice || model.OrderStatusId != orderBefor.OrderStatusId)
                    {
                        confirmChange = true;
                        orderUpdateHistory.Active = 1;
                        orderUpdateHistory.OrderId = model.Id;
                        orderUpdateHistory.AccountId = this.GetLoggedInUserId();
                        var accountChange = await accountService.GetInfoAccountById(orderUpdateHistory.AccountId);
                        orderUpdateHistory.Change += "Người thay đổi: " + accountChange.FullName + " ( " + accountChange.RoleName + " );";
                        var historyOrderOld = new HistoryOrder();
                        historyOrderOld.FinalPrice = orderBefor.FinalPrice == null ? 0 : orderBefor.FinalPrice;
                        historyOrderOld.Price = orderBefor.Price == null ? 0 : orderBefor.Price;
                        historyOrderOld.OrderStatus = (await orderStatusService.Detail(orderBefor.OrderStatusId))[0].Name;
                        orderUpdateHistory.ObjectOld = JsonConvert.SerializeObject(historyOrderOld);

                        var historyOrderNew = new HistoryOrder();
                        historyOrderNew.FinalPrice = model.FinalPrice == null ? 0 : model.FinalPrice;
                        historyOrderNew.Price = model.Price == null ? 0 : model.Price;
                        historyOrderNew.OrderStatus = (await orderStatusService.Detail(model.OrderStatusId))[0].Name;
                        orderUpdateHistory.ObjectNew = JsonConvert.SerializeObject(historyOrderNew);

                        if (historyOrderNew.FinalPrice != historyOrderOld.FinalPrice)
                        {
                           
                            orderUpdateHistory.Change += "Giá cuối: " + (historyOrderOld.FinalPrice == 0 ? "0" : String.Format("{0:0,0}", historyOrderOld.FinalPrice)) + " -> " + (historyOrderNew.FinalPrice == 0 ? "0" : String.Format("{0:0,0}", historyOrderNew.FinalPrice)) + ";";

                        }
                        if (historyOrderNew.Price != historyOrderOld.Price)
                        {
                            orderUpdateHistory.Change += "Giá trần: " + (historyOrderOld.Price == 0 ? "0" : String.Format("{0:0,0}", historyOrderOld.Price)) + " -> " + (historyOrderNew.FinalPrice == 0 ? "0" : String.Format("{0:0,0}", historyOrderNew.Price)) + ";";
                        }
                        if (orderBefor.OrderStatusId != model.OrderStatusId)
                        {
                            orderUpdateHistory.Change += "Trạng thái: " + historyOrderOld.OrderStatus + " -> " + historyOrderNew.OrderStatus + ";";
                        }
                        orderUpdateHistory.CreatedTime = DateTime.Now;
                    }                  
                    await service.UpdateByViewModel(model, orderUpdateHistory, confirmChange);
                    var orderAfter = await service.Detail2(model.Id);
                    try
                    {
                        if(orderBefor.OrderStatusId != OrderStatusId.PURCHAED && model.OrderStatusId == OrderStatusId.PURCHAED)
                        {
                            List<DeviceToken> listGuidIds = await accountService.GetDeviceToken();
                            foreach (DeviceToken guidIds in listGuidIds)
                            {
                                if (!string.IsNullOrEmpty(guidIds.GuidId))
                                {
                                    FCMExtension.SendNotificationToMobile(guidIds.GuidId, "Biển " + orderAfter.ProductName + " đã được mua", "Đã mua biển " + orderAfter.ProductName + " với giá cuối là " + String.Format("{0:0,0}", orderAfter.FinalPrice) + " VNĐ", NotificationKey.KEY_GET_NOTIFICATION, model.Id);
                                }
                                if (!string.IsNullOrEmpty(guidIds.GuidIdApp))
                                {
                                    FCMExtension.SendNotificationToMobile(guidIds.GuidIdApp, "Biển " + orderAfter.ProductName + " đã được mua", "Đã mua biển " + orderAfter.ProductName + " với giá cuối là " + String.Format("{0:0,0}", orderAfter.FinalPrice) + " VNĐ", NotificationKey.KEY_GET_NOTIFICATION, model.Id);
                                }
                            }
                        }
                    }catch(Exception e)
                    {

                    }
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
        [HttpGet]
        [Route("api/Detail2/{Id}")]
        public async Task<IActionResult> Detail2(int Id)
        {
            try
            {
                var data = await service.Detail2(Id);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("api/Delete")]
        public async Task<IActionResult> Delete([FromBody] Order model)
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
        public async Task<IActionResult> DeletePermanently([FromBody] Order model)
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
        public int CountOrders()
        {
            int result = service.Count();
            return result;
        }
        [HttpPost]
        [Route("api/list-server-side")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ListServerSide([FromBody] OrdersDTParameters parameters)
        {
            try
            {
                int accountId = this.GetLoggedInUserId();
                var data = await service.ListServerSide(parameters, accountId);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        [Route("api/get-total-orderstatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTotalOrderStatus()
        {
            try
            {
                int accountId = this.GetLoggedInUserId();
                var data = await service.GetTotalOrderStatus(accountId);
                return Ok(data);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("api/list-paging")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ListPaging([FromBody] OrderPagingRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = await service.ListPagingSort(model);
                    return Ok(data);
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("api/change-order-status")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, int confirm)
        {
            try
            {
                Order model = new Order();
                model.Id = orderId;
                model.OrderStatusId = confirm == 0 ? OrderStatusId.CANCEL : OrderStatusId.APPROVED;
                await service.ChangeOrderStatus(model);
                return Ok(model);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("api/update-order-sort")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrder model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var data = await service.UpdateOrderSort(model);
                    if (data)
                    {
                        var QLBH_DionResponse = QLBH_Response.SUCCESS(model);
                        return Ok(QLBH_DionResponse);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("api/export-excel")]
        public async Task<IActionResult> ExportExcel(string id, string productName, string price, string registerClosedTime, string auctionTime, string accountName,string orderStatusId, string createdTime)
        {
           
            try
            {
                FillterExportOrderExcel fillter = new FillterExportOrderExcel();
                fillter.Id = id == null? "" : id;
                fillter.ProductName = productName == null? "" : productName;
                fillter.Price = price == null? "" : price;
                fillter.RegisterClosedTime = registerClosedTime == null ? "" : registerClosedTime;
                fillter.AuctionTime = auctionTime == null? "" : auctionTime;
                fillter.AccountName = accountName == null? "" : accountName;
                string tempOrderStatusId = orderStatusId ?? "";
                fillter.OrderStatusId = orderStatusId == null ?   new string[0] : tempOrderStatusId.Split(',');
                fillter.CreatedTime = createdTime == null ? "" : createdTime ;
                // Danh sách SV
                var data = await service.ExportOrdersExcel(fillter);
                if(data != null)
                {
                    XSSFWorkbook wb = new XSSFWorkbook();
                    // style column
                    ICellStyle style = wb.CreateCellStyle();
                    style.BorderBottom = BorderStyle.Thin;
                    style.BottomBorderColor = HSSFColor.Black.Index;
                    style.BorderTop = BorderStyle.Thin;
                    style.TopBorderColor = HSSFColor.Black.Index;
                    style.BorderLeft = BorderStyle.Thin;
                    style.LeftBorderColor = HSSFColor.Black.Index;
                    style.BorderRight = BorderStyle.Thin;
                    style.RightBorderColor = HSSFColor.Black.Index;
                    //style.FillPattern = FillPattern.SolidForeground;
                    //style.FillForegroundColor = IndexedColors.LightGreen.Index;
                    style.Alignment = HorizontalAlignment.Center;
                    style.VerticalAlignment = VerticalAlignment.Center;
                    style.WrapText = true;
                    var font1 = wb.CreateFont();
                    font1.IsBold = true;
                    style.SetFont(font1);

                    //style chung
                    //border
                    ICellStyle styleBorder = wb.CreateCellStyle();
                    styleBorder.BorderBottom = BorderStyle.Thin;
                    styleBorder.BottomBorderColor = HSSFColor.Black.Index;
                    styleBorder.BorderTop = BorderStyle.Thin;
                    styleBorder.TopBorderColor = HSSFColor.Black.Index;
                    styleBorder.BorderLeft = BorderStyle.Thin;
                    styleBorder.LeftBorderColor = HSSFColor.Black.Index;
                    styleBorder.BorderRight = BorderStyle.Thin;
                    styleBorder.RightBorderColor = HSSFColor.Black.Index;
                  

                    //number bold
                    ICellStyle styleNumberBold = wb.CreateCellStyle();
                    styleNumberBold.BorderBottom = BorderStyle.Thin;
                    styleNumberBold.BottomBorderColor = HSSFColor.Black.Index;
                    styleNumberBold.BorderTop = BorderStyle.Thin;
                    styleNumberBold.TopBorderColor = HSSFColor.Black.Index;
                    styleNumberBold.BorderLeft = BorderStyle.Thin;
                    styleNumberBold.LeftBorderColor = HSSFColor.Black.Index;
                    styleNumberBold.BorderRight = BorderStyle.Thin;
                    styleNumberBold.RightBorderColor = HSSFColor.Black.Index;
                    styleNumberBold.SetFont(font1);

                    //format number normal
                    ICellStyle formatNumberNormal = wb.CreateCellStyle();
                    formatNumberNormal.BorderBottom = BorderStyle.Thin;
                    formatNumberNormal.BottomBorderColor = HSSFColor.Black.Index;
                    formatNumberNormal.BorderTop = BorderStyle.Thin;
                    formatNumberNormal.TopBorderColor = HSSFColor.Black.Index;
                    formatNumberNormal.BorderLeft = BorderStyle.Thin;
                    formatNumberNormal.LeftBorderColor = HSSFColor.Black.Index;
                    formatNumberNormal.BorderRight = BorderStyle.Thin;
                    formatNumberNormal.RightBorderColor = HSSFColor.Black.Index;
                    formatNumberNormal.DataFormat = wb.CreateDataFormat().GetFormat(string.Format("#,##"));

                    //format number bold
                    ICellStyle formatNumberBold = wb.CreateCellStyle();
                    formatNumberBold.BorderBottom = BorderStyle.Thin;
                    formatNumberBold.BottomBorderColor = HSSFColor.Black.Index;
                    formatNumberBold.BorderTop = BorderStyle.Thin;
                    formatNumberBold.TopBorderColor = HSSFColor.Black.Index;
                    formatNumberBold.BorderLeft = BorderStyle.Thin;
                    formatNumberBold.LeftBorderColor = HSSFColor.Black.Index;
                    formatNumberBold.BorderRight = BorderStyle.Thin;
                    formatNumberBold.RightBorderColor = HSSFColor.Black.Index;
                    formatNumberBold.DataFormat = wb.CreateDataFormat().GetFormat(string.Format("#,##"));
                    formatNumberBold.SetFont(font1);

                    //style title
                    ICellStyle styleTitle = wb.CreateCellStyle();
                    styleTitle.Alignment = HorizontalAlignment.Center;
                    styleTitle.VerticalAlignment = VerticalAlignment.Center;
                    styleTitle.WrapText = true;
                    styleTitle.SetFont(font1);
                    // Tạo ra 1 sheet
                    ISheet sheet = wb.CreateSheet("Danh sách yêu cầu");
                    // Tạo row
                    var row0 = sheet.CreateRow(0);
                    // Merge lại row đầu 3 cột
                    row0.CreateCell(0); // tạo ra cell trc khi merge
                    CellRangeAddress cellMerge = new CellRangeAddress(0, 0, 0, 8);
                    sheet.AddMergedRegion(cellMerge);
                    row0.GetCell(0).SetCellValue("Danh sách yêu cầu");
                    row0.GetCell(0).CellStyle = styleTitle;

                    // Ghi tên cột ở row 1
                    var row1 = sheet.CreateRow(1);
                    ICell cellSTT = row1.CreateCell(0);
                    cellSTT.SetCellValue("STT");
                    ICell cellId = row1.CreateCell(1);
                    cellId.SetCellValue("Mã yêu cầu");
                    ICell cellProductName = row1.CreateCell(2);
                    cellProductName.SetCellValue("Biển số");
                    ICell cellPrice = row1.CreateCell(3);
                    cellPrice.SetCellValue("Giá trần");
                    ICell cellClosedRegister = row1.CreateCell(4);
                    cellClosedRegister.SetCellValue("Thời gian kết thúc đăng ký");
                    ICell cellAuctionDate = row1.CreateCell(5);
                    cellAuctionDate.SetCellValue("Ngày đấu giá");
                    ICell cellAuctionTime = row1.CreateCell(6);
                    cellAuctionTime.SetCellValue("Thời gian đấu giá");
                    ICell cellAccountName = row1.CreateCell(7);
                    cellAccountName.SetCellValue("Người yêu cầu");
                    ICell cellOrderStatusName = row1.CreateCell(8);
                    cellOrderStatusName.SetCellValue("Trạng thái yêu cầu");
                    ICell cellCreatedTime = row1.CreateCell(9);
                    cellCreatedTime.SetCellValue("Ngày tạo");
                    cellSTT.CellStyle = style;
                    cellId.CellStyle = style;
                    cellProductName.CellStyle = style;
                    cellPrice.CellStyle = style;
                    cellClosedRegister.CellStyle = style;
                    cellAuctionTime.CellStyle = style;
                    cellAuctionDate.CellStyle = style;
                    cellAccountName.CellStyle = style;
                    cellOrderStatusName.CellStyle = style;
                    cellCreatedTime.CellStyle = style;
                    int rowIndex = 2;
                    int stt = 1;
                    
                    foreach (var item in data)
                    {
                        // tao row mới
                        var newRow = sheet.CreateRow(rowIndex);
                        ICell cellSTT1 = newRow.CreateCell(0);
                        cellSTT1.SetCellValue(stt);
                        ICell cellId1 = newRow.CreateCell(1);
                        cellId1.SetCellValue("#" + item.Id);
                        ICell cellProductName1 = newRow.CreateCell(2);
                        cellProductName1.SetCellValue(item.ProductName);
                        ICell cellPrice1 = newRow.CreateCell(3);
                        double priceExcel = item.Price == null ? 0 : (double)item.Price;
                        cellPrice1.SetCellValue(priceExcel);
                        ICell cellClosedRegister1 = newRow.CreateCell(4);
                        cellClosedRegister1.SetCellValue(item.RegisterClosedTime.ToString("dd/MM/yyyy"));
                        ICell cellAuction1 = newRow.CreateCell(5);
                        var displayOpenTime = item.OpenTime.ToString("dd/MM/yyyy HH:mm").Split(' ');
                        var displayClosedTime = item.ClosedTime.ToString("dd/MM/yyyy HH:mm").Split(' ');
                        cellAuction1.SetCellValue(displayOpenTime[0]);
                        ICell cellAuctionTime1 = newRow.CreateCell(6);
                        cellAuctionTime1.SetCellValue(displayOpenTime[1] + "-" + displayClosedTime[1]);
                        ICell cellAccountName1 = newRow.CreateCell(7);
                        cellAccountName1.SetCellValue(item.AccountName);
                        ICell cellOrderStatusName1 = newRow.CreateCell(8);
                        cellOrderStatusName1.SetCellValue(item.OrderStatusName);
                        ICell cellCreatedTime1 = newRow.CreateCell(9);
                        cellCreatedTime1.SetCellValue(item.CreatedTime.ToString("dd/MM/yyyy HH:mm:ss"));
                        cellSTT1.CellStyle = styleBorder;
                        cellId1.CellStyle = styleBorder;
                        cellProductName1.CellStyle = styleBorder;
                        cellPrice1.CellStyle = formatNumberNormal;
                        cellClosedRegister1.CellStyle = styleBorder;
                        cellAuction1.CellStyle = styleBorder;
                        cellAccountName1.CellStyle = styleBorder;
                        cellOrderStatusName1.CellStyle = styleBorder;
                        cellCreatedTime1.CellStyle = styleBorder;
                        cellAuctionTime1.CellStyle = styleBorder;
                       
                        rowIndex++;
                        stt++;
                    }
                    for (int j = 0; j < 10; j++)
                    {
                        sheet.AutoSizeColumn(j);
                    }
                    string fileName = "DanhSachYeuCau.xlsx";
                    using (var memory = new MemoryStream())
                    {
                        wb.Write(memory);
                        var byte_array = memory.ToArray();
                        return File(byte_array, "application/vnd.ms-excel",fileName);
                    }
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/get-all-order")]
        public async Task<IActionResult> GetAllProductDetail()
        {
            try
            {
                var data = await service.GetAllProductDetail();
                return Ok(data);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("api/get-detail-order-sort/{Id}")]
        public async Task<IActionResult> GetDetailOrderSort(int Id)
        {
            try
            {
                var data = await service.GetDetailOderSort(Id);
                return Ok(data);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }
    }
}
