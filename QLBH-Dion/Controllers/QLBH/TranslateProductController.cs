using HtmlAgilityPack;
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
using NPOI.XWPF.UserModel;
using System.ComponentModel;

namespace QLBH_Dion.Controllers.QLBH
{
    [Route("[controller]")]
    [ApiController]
    public class TranslateProductController : BaseController
    {
        ITranslateProductService service;
        public TranslateProductController(ITranslateProductService _service)
        {
            service = _service;
        }

        [HttpGet]
        [Route("api/translate-product")]
        public async Task<IActionResult> TranslateProduct(string name)
        {
            try
            {
                string url = "https://dichbiensoxe.com/dich-bien-so/dich-bien-so-xe-" + name;
                string htmlContent = await service.GetHtmlContentAsync(url);
                string childText = service.HandleDoubleChar(name);
                if (!string.IsNullOrEmpty(htmlContent))
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlContent);

                    // Xóa header
                    var headerNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='header']");
                    headerNode.Remove();

                    // Xóa Comment
                    var commentsNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='comments']");
                    commentsNode.Remove();

                    // Xóa footer
                    var footerNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='footer']");
                    footerNode.Remove();

                 

                    // Xóa right 
                    var secondaryNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='secondary']");
                    secondaryNode.Remove();
                    var custom3Node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='custom_html-3']");
                    custom3Node.Remove();
                    var custom10Node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='custom_html-10']");
                    custom10Node.Remove();

                    // Xóa tất cả href của thẻ a ngoại trừ 1 số 
                    var aNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (aNodes != null)
                    {
                        // 3 href nuwsa chua xu li
                        var allowedHrefs = new HashSet<string> {
                            "#Dich_nghia_bien_so_xe_"+name,
                            "#Dinh_gia_bien_so_xe_"+name,
                            "#Danh_gia_do_phu_hop_cua_bien_so_"+name+"_voi_ngu_hanh",
                            "#Ket_qua_phan_tich_y_nghia_bien_so_xe_" + name + "_chi_tiet",
                            "#Dua_theo_ngu_hanh_giai_ma_bien_so_xe_"+name,
                            "#Cac_cach_ghep_nghia_bien_so_xe_"+name,
                            "#Y_nghia_cac_con_so_phong_thuy_" + childText + "tren_bien_so_xe_"+ name,
                            "#Y_nghia_cua_bien_so_xe_"+name+"_trong_kinh_dich"
                        };
                        foreach (var node in aNodes)
                        {
                            var hrefAttribute = node.GetAttributeValue("href", null);

                            // Kiểm tra xem href có trong danh sách allowedHrefs không
                            if (!allowedHrefs.Contains(hrefAttribute))
                            {
                                node.Attributes.Remove("href");
                            }
                        }
                    }

                    // Xóa toàn bộ thẻ img
                    var imgNodes = htmlDoc.DocumentNode.SelectNodes("//img");
                    if (imgNodes != null)
                    {
                        foreach (var node in imgNodes)
                        {
                            node.Remove();
                        }

                    }

                    // Xóa class
                    var pulseNode = htmlDoc.DocumentNode.SelectNodes("//*[@class='pulse-button']");
                    if (pulseNode != null)
                    {
                        foreach (var node in pulseNode)
                        {
                            node.Remove();
                        }
                    }
                    // Xoas facebook share
                    var facebookNode = htmlDoc.DocumentNode.SelectNodes("//*[@class='facebook-share']");
                    if (facebookNode != null)
                    {
                        foreach (var node in facebookNode)
                        {
                            node.Remove();
                        }
                    }

                    var a = htmlDoc.DocumentNode.OuterHtml.Trim();
                    return View("TranslateProduct", htmlDoc.DocumentNode.OuterHtml.Trim());

                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        [HttpGet]
        [Route("api/translate-product-iframe")]
        public async Task<string> TranslateProductIFrame(string name)
        {
            try
            {
                string url = "https://dichbiensoxe.com/dich-bien-so/dich-bien-so-xe-" + name;
                string htmlContent = await service.GetHtmlContentAsync(url);
                string childText = service.HandleDoubleChar(name);
                if (!string.IsNullOrEmpty(htmlContent))
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(htmlContent);
                   

                    // Xóa header
                    var headerNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='header']");
                    headerNode.Remove();

                    // Xóa Comment
                    var commentsNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='comments']");
                    commentsNode.Remove();

                    // Xóa footer
                    var footerNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='footer']");
                    footerNode.Remove();

                    // Xóa right 
                    var secondaryNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='secondary']");
                    secondaryNode.Remove();
                    var custom3Node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='custom_html-3']");
                    custom3Node.Remove();
                    var custom10Node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='custom_html-10']");
                    custom10Node.Remove();

                    // Xóa tất cả href của thẻ a ngoại trừ 1 số 
                    var aNodes = htmlDoc.DocumentNode.SelectNodes("//a");
                    if (aNodes != null)
                    {
                        // 3 href nuwsa chua xu li
                        var allowedHrefs = new HashSet<string> {
                            "#Dich_nghia_bien_so_xe_"+name,
                            "#Dinh_gia_bien_so_xe_"+name,
                            "#Danh_gia_do_phu_hop_cua_bien_so_"+name+"_voi_ngu_hanh",
                            "#Ket_qua_phan_tich_y_nghia_bien_so_xe_" + name + "_chi_tiet",
                            "#Dua_theo_ngu_hanh_giai_ma_bien_so_xe_"+name,
                            "#Cac_cach_ghep_nghia_bien_so_xe_"+name,
                            "#Y_nghia_cac_con_so_phong_thuy_" + childText + "tren_bien_so_xe_"+ name,
                            "#Y_nghia_cua_bien_so_xe_"+name+"_trong_kinh_dich"
                        };
                        foreach (var node in aNodes)
                        {
                            var hrefAttribute = node.GetAttributeValue("href", null);

                            // Kiểm tra xem href có trong danh sách allowedHrefs không
                            if (!allowedHrefs.Contains(hrefAttribute))
                            {
                                node.Attributes.Remove("href");
                            }
                        }
                    }

                    // Xóa toàn bộ thẻ img
                    var imgNodes = htmlDoc.DocumentNode.SelectNodes("//img");
                    if (imgNodes != null)
                    {
                        foreach (var node in imgNodes)
                        {
                            node.Remove();
                        }

                    }

                    // Xóa class
                    var pulseNode = htmlDoc.DocumentNode.SelectNodes("//*[@class='pulse-button']");
                    if (pulseNode != null)
                    {
                        foreach (var node in pulseNode)
                        {
                            node.Remove();
                        }
                    }
                    // Xoas facebook share
                    var facebookNode = htmlDoc.DocumentNode.SelectNodes("//*[@class='facebook-share']");
                    if (facebookNode != null)
                    {
                        foreach (var node in facebookNode)
                        {
                            node.Remove();
                        }
                    }

                    return htmlDoc.DocumentNode.OuterHtml.Trim();

                }

                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
