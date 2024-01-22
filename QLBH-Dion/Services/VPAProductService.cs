using iText.Kernel.Geom;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using QLBH_Dion.Models;
using QLBH_Dion.Repository;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util;
using QLBH_Dion.Util.VPAProduct;
using System.Globalization;
using Product = QLBH_Dion.Models.Product;

namespace QLBH_Dion.Services
{
    public class VPAProductService : IVPAProductService
    {
        private readonly IConfiguration configuration;
        IProductRepository productRepository;
        IAuctionProductRepository auctionProductRepository;
        ILogger<VPAProductService> logger;
        public VPAProductService(IConfiguration _configuration, IProductRepository _productRepository, IAuctionProductRepository _auctionProductRepository, ILoggerFactory loggerFactory)
        {
            configuration = _configuration;
            productRepository = _productRepository;
            auctionProductRepository = _auctionProductRepository;
            logger = loggerFactory.CreateLogger<VPAProductService>();
        }
        public async Task<VPAProductResponse> GetProductByPageAndStatus(VPAProductRequest request)
        {
            using HttpClient httpClient = new HttpClient();
            var setting = configuration.GetSection("VPAProductSetting").Get<VPAProductSetting>();
            var url = setting.Url;
            url = url.Replace("{page}", request.page).Replace("{plate_status}", request.plate_status);
            httpClient.DefaultRequestHeaders.Add("authority", "vpa.com.vn");
            httpClient.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            httpClient.DefaultRequestHeaders.Add("accept-language", "en-US,en;q=0.9");
            httpClient.DefaultRequestHeaders.Add("cache-control", "max-age=0");
            httpClient.DefaultRequestHeaders.Add("cookie", "_ga=GA1.1.516078341.1700530656; _ga_R1M98GBK4S=GS1.1.1703228579.23.0.1703228579.0.0.0; cf_clearance=nfRF53ihmithVkHNa6uFsti9q_scoW5Z3cXBFuBpi0A-1703473759-0-2-92bcd2d9.e49319a9.c5a410a3-0.2.1703473759; _ga_QVGFBK5EKL=GS1.1.1703479023.13.0.1703479023.0.0.0; __cf_bm=Gru5i1Gi3ObhmYm.NSVxBOlNp0nrM0WgpUOdFR2Elok-1703479426-1-AV8MIIsHyNCjA/0Po61rqpPfv8YcwvZRhOYCrcRrkCfpEOdhfK5PdE78LB2BRxtQRzvwVHo1jtndx0OnYTinFR4=; __cf_bm=WqTbCQYSoFlRfIJ6SF2uIYrTTwgi3mq4o_c7L70tA4Q-1703481011-1-ATE3VCCgK9FlJRVADpV3YM1prFpMJxjYDibFuV9CeKSSITaW87s06m6BLWGQgXSB6tcHsc2n9htOY1yZ7Udn+98=");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "document");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "navigate");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "none");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-user", "?1");
            httpClient.DefaultRequestHeaders.Add("upgrade-insecure-requests", "1");
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            //var response = await httpClient.SendRequestAsync<GHTKCreateOrderRequest, GHTKCreateOrderResponse>(url, request);
            var response = await httpClient.GetRequestAsync<VPAProductResponse>(url);
            return response;
        }

        public async Task<bool> SynchronizedProduct()
        {
            try
            {
                using (var trans = await auctionProductRepository.GetDatabase().BeginTransactionAsync())
                {
                    var now = DateTime.Now;
                    var listProduct = await productRepository.List();
                    var allAuctionProduct = await auctionProductRepository.List();

                    var totalPage = 0;
                    var request = new VPAProductRequest()
                    {
                        page = "1",
                        plate_status = "waiting_auction"
                    };
                    var dataFirstPage = await GetProductByPageAndStatus(request);
                    //Lấy ra tổng số page
                    if (dataFirstPage.data.Count > 0)
                    {
                        totalPage = dataFirstPage.total % dataFirstPage.data.Count > 0 ? dataFirstPage.total / dataFirstPage.data.Count + 1 : dataFirstPage.total / dataFirstPage.data.Count;
                    }
                    var listVPAProduct = new List<Util.VPAProduct.Product>();

                    for (int i = totalPage; i >= 1; i--)
                    {
                        request.page = i.ToString();
                        var NewPagedata = await GetProductByPageAndStatus(request);
                        if (NewPagedata.data != null)
                        {
                            if (NewPagedata.data.Count > 0)
                            {
                                listVPAProduct.AddRange(NewPagedata.data);
                            }
                        }
                 
                    }
                    var listAuctionProduct = new List<AuctionProduct>();
                    var listUpdateProduct = new List<AuctionProduct>();

                    for (int j = 0; j < listVPAProduct.Count; j++)
                    {
                        var prd = listVPAProduct[j];
                        var nameProduct = prd.bks.Insert(3, "-").Insert(prd.bks.Length - 1, ".");
                        var registerOpenTime = DateTime.ParseExact(prd.registerFromTime, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                        var registerClosedTime = DateTime.ParseExact(prd.registerToTime, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                        var auctionOpenTime = DateTime.ParseExact(prd.auctionStartTime, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                        var auctionEndTime = DateTime.ParseExact(prd.auctionEndTime, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                        var productDetail = listProduct.FirstOrDefault(x => x.Name == nameProduct && x.Active == 1);
                        var auctionStatus = SystemConstant.AUCTION_PRODUCT_STATUS_ID_ENDED;
                        if (now < auctionOpenTime)
                        {
                            auctionStatus = SystemConstant.AUCTION_PRODUCT_STATUS_ID_WAITING;
                        }
                        if (auctionOpenTime < now && now < auctionEndTime)
                        {
                            auctionStatus = SystemConstant.AUCTION_PRODUCT_STATUS_ID_GOING;
                        }
                        //Nếu chưa có thì add
                        if (productDetail == null)
                        {
                            var auctionProduct = new AuctionProduct()
                            {
                                AuctionProductStatusId = auctionStatus,
                                RegisterOpenTime = registerOpenTime,
                                RegisterClosedTime = registerClosedTime,
                                OpenTime = auctionOpenTime,
                                ClosedTime = auctionEndTime,
                                Code = prd.id,
                                Active = 1,
                                CreatedTime = now,
                                Product = new Product()
                                {
                                    Name = nameProduct,
                                    ProductCategoryId = prd.loai_bien.id,
                                    ProductBrandId = prd.loai_xe.id,
                                    ProvinceId = prd.tinh.id,
                                    Code = prd.id,
                                    Active = 1,
                                    CreatedTime = now,
                                    Description = JsonConvert.SerializeObject(prd)
                                }
                            };
                            listAuctionProduct.Add(auctionProduct);
                        }
                        else
                        {
                            var auctionProduct = allAuctionProduct.FirstOrDefault(x => x.ProductId == productDetail.Id && x.Active == 1);
                            //Nếu có auctionProduct thì add vào list update
                            if (auctionProduct != null)
                            {
                                if (auctionProduct.RegisterOpenTime != registerOpenTime
                                    || auctionProduct.RegisterClosedTime != registerClosedTime
                                    || auctionProduct.OpenTime != auctionOpenTime
                                    || auctionProduct.ClosedTime != auctionEndTime
                                    || auctionProduct.AuctionProductStatusId != auctionStatus)
                                {
                                    auctionProduct.RegisterOpenTime = registerOpenTime;
                                    auctionProduct.RegisterClosedTime = registerClosedTime;
                                    auctionProduct.OpenTime = auctionOpenTime;
                                    auctionProduct.ClosedTime = auctionEndTime;
                                    auctionProduct.AuctionProductStatusId = auctionStatus;
                                    listUpdateProduct.Add(auctionProduct);
                                }
                            }
                            //Nếu chưa có thì add vào list add
                            else
                            {
                                var newAuctionProduct = new AuctionProduct()
                                {
                                    AuctionProductStatusId = auctionStatus,
                                    RegisterOpenTime = registerOpenTime,
                                    RegisterClosedTime = registerClosedTime,
                                    OpenTime = auctionOpenTime,
                                    ClosedTime = auctionEndTime,
                                    Code = prd.id,
                                    Active = 1,
                                    CreatedTime = now,
                                    ProductId = productDetail.Id
                                };
                                listAuctionProduct.Add(newAuctionProduct);
                            }
                        }
                    }
                    //Update trạng thái đáu giá
                    for (int i = 0; i < allAuctionProduct.Count; i++)
                    {
                            var statusId = SystemConstant.AUCTION_PRODUCT_STATUS_ID_ENDED;
                            if (allAuctionProduct[i].OpenTime > now)
                            {
                                statusId = SystemConstant.AUCTION_PRODUCT_STATUS_ID_WAITING;
                            }
                            else if (allAuctionProduct[i].OpenTime < now && now < allAuctionProduct[i].ClosedTime)
                            {
                                statusId = SystemConstant.AUCTION_PRODUCT_STATUS_ID_GOING;
                            }
                            if (allAuctionProduct[i].AuctionProductStatusId != statusId && listUpdateProduct.Find(x => x.Id == allAuctionProduct[i].Id) == null)
                            {
                                allAuctionProduct[i].AuctionProductStatusId = statusId;
                                listUpdateProduct.Add(allAuctionProduct[i]);
                            }                  
                    }
                    await auctionProductRepository.AddRange(listAuctionProduct);
                    await auctionProductRepository.UpdateRange(listUpdateProduct);
                    await trans.CommitAsync();
                    logger.LogInformation($"Synchronized Success!");
                }
            }
            catch (Exception e)
            {
                logger.LogInformation(e, $"Synchronized fail!");
                return false;

            }
            return true;
        }
    }
}

