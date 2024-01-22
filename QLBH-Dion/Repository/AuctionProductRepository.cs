
using QLBH_Dion.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Data.SqlClient;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Repository
{
    public class AuctionProductRepository : IAuctionProductRepository
    {
        QLBHContext db;
        public AuctionProductRepository(QLBHContext _db)
        {
            db = _db;
        }


        public async Task<List<AuctionProduct>> List()
        {
            if (db != null)
            {
                return await (
                    from row in db.AuctionProducts
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }

            return null;
        }


        public async Task<List<AuctionProduct>> Search(string keyword)
        {
            if (db != null)
            {
                return await (
                    from row in db.AuctionProducts
                    where (row.Active == 1 && (row.Code.Contains(keyword) || row.Description.Contains(keyword)))
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }
            return null;
        }


        public async Task<List<AuctionProduct>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await (
                    from row in db.AuctionProducts
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }


        public async Task<List<AuctionProduct>> Detail(int? id)
        {
            if (db != null)
            {
                return await (
                    from row in db.AuctionProducts
                    where (row.Active == 1 && row.Id == id)
                    select row)
                    .ToListAsync();
            }

            return null;
        }

        public async Task<List<AuctionProduct>> ListByProductId(int productId)
        {
            if (db != null)
            {
                return await (
                    from row in db.AuctionProducts
                    where (row.Active == 1 && row.ProductId == productId)
                    select row)
                    .ToListAsync();
            }

            return null;
        }
        public async Task<AuctionProduct> Add(AuctionProduct obj)
        {
            if (db != null)
            {
                await db.AuctionProducts.AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            return null;
        }

        public async Task AddRange(List<AuctionProduct> AuctionProduct)
        {
            if (db != null)
            {
                await db.AuctionProducts.AddRangeAsync(AuctionProduct);
                await db.SaveChangesAsync();
            }
        }
        public async Task UpdateRange(List<AuctionProduct> products)
        {
            foreach (var product in products)
            {
                db.Attach(product);
                db.Entry(product).Property(x => x.RegisterOpenTime).IsModified = true;
                db.Entry(product).Property(x => x.RegisterClosedTime).IsModified = true;
                db.Entry(product).Property(x => x.OpenTime).IsModified = true;
                db.Entry(product).Property(x => x.ClosedTime).IsModified = true;
                db.Entry(product).Property(x => x.AuctionProductStatusId).IsModified = true;
            }
            await db.SaveChangesAsync();
        }
        public async Task Update(AuctionProduct obj)
        {
            if (db != null)
            {
                //Update that object
                db.AuctionProducts.Attach(obj);
                db.Entry(obj).Property(x => x.ProductId).IsModified = true;
                //db.Entry(obj).Property(x => x.AuctionProductId).IsModified = true;
                db.Entry(obj).Property(x => x.FinalPrice).IsModified = true;
                db.Entry(obj).Property(x => x.RegisterOpenTime).IsModified = true;
                db.Entry(obj).Property(x => x.RegisterClosedTime).IsModified = true;
                db.Entry(obj).Property(x => x.OpenTime).IsModified = true;
                db.Entry(obj).Property(x => x.ClosedTime).IsModified = true;
                db.Entry(obj).Property(x => x.Code).IsModified = true;
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                db.Entry(obj).Property(x => x.Active).IsModified = true;
                db.Entry(obj).Property(x => x.Search).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }


        public async Task Delete(AuctionProduct obj)
        {
            if (db != null)
            {
                //Update that obj
                db.AuctionProducts.Attach(obj);
                db.Entry(obj).Property(x => x.Active).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }

        public async Task<int> DeletePermanently(int? objId)
        {
            int result = 0;

            if (db != null)
            {
                //Find the obj for specific obj id
                var obj = await db.AuctionProducts.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.AuctionProducts.Remove(obj);

                    //Commit the transaction
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }


        public int Count()
        {
            int result = 0;

            if (db != null)
            {
                //Find the obj for specific obj id
                result = (
                    from row in db.AuctionProducts
                    where row.Active == 1
                    select row
                            ).Count();
            }

            return result;
        }
        public async Task<AuctionProductPaging> ListPagingApp(AuctionProductPagingRequest model)
        {
            //1. get all data
            int offSet = 0;
            offSet = (model.pageIndex - 1) * model.pageSize;
            var query = from row in db.AuctionProducts
                        join p in db.Products on row.ProductId equals p.Id
                        join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                        join pro in db.Provinces on p.ProvinceId equals pro.Id
                        join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                        join aps in db.AuctionProductStatuses on row.AuctionProductStatusId equals aps.Id
                        where row.Active == 1
                        && p.Active == 1
                        && pc.Active == 1
                        select new AuctionProductSort
                        {
                            Id = row.Id,
                            productId = p.Id,
                            productName = p.Name,
                            ProductCategoryId = p.ProductCategoryId,
                            ProductCategoryName = pc.Name,
                            ProvinceId = p.ProvinceId,
                            ProvinceName = pro.Name,
                            ProductBrandId = p.ProductBrandId,
                            ProductBrandName = pb.Name,
                            OpenTime = row.OpenTime,
                            ClosedTime = row.ClosedTime,
                            registerClosedTime = row.RegisterClosedTime,
                            productStatusId = row.AuctionProductStatusId,
                            productStatusName = aps.Name
                        };
            if (!String.IsNullOrEmpty(model.keyword))
            {
                model.keyword = model.keyword.Replace("-", "").Replace(".", "").ToLower();
                try
                {
                    query = query.Where(c =>
                    EF.Functions.Collate(c.productName.Replace("-", "").Replace(".", "").ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(model.keyword, SQLParams.Latin_General))
                );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            if(model.ProductName != null)
            {
                var filter = model.ProductName.Trim().Replace(".", "").Replace("-", "").ToLower();
                if (filter.Contains("*"))
                {
                    var headCharacter = filter.Substring(0, filter.IndexOf("*"));
                    var tailCharacter = filter.Substring(filter.IndexOf("*") + 1);
                    string partern = $"{headCharacter}%{tailCharacter}";
                    query = query.Where(c => EF.Functions.Like(c.productName.Trim().Replace(".", "").Replace("-", "").ToLower(), partern));
                }
                else
                {
                    query = query.Where(c => c.productName.Trim().Replace(".", "").Replace("-", "").ToLower().Contains(filter));
                }
            }
            if (model.AuctionProductCategoryId != null)
            {
                query = query.Where(c => c.ProductCategoryId == model.AuctionProductCategoryId);
            }
            if (model.ProvinceId != null)
            {
                query = query.Where(c => c.ProvinceId == model.ProvinceId);
            }
            if (model.AuctionProductCategoryId != null)
            {
                query = query.Where(c => c.ProductCategoryId == model.AuctionProductCategoryId);
            }
            if (model.AuctionProductBrandId != null)
            {
                query = query.Where(c => c.ProductBrandId == model.AuctionProductBrandId);
            }
            // Lọc theo từ ngày đến ngày
            if (model.FromRegisterClosedTime != null)
            {
                query = query.Where(c => c.registerClosedTime >= DateTime.ParseExact(model.FromRegisterClosedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            if (model.ToRegisterClosedTime != null)
            {
                query = query.Where(c => c.registerClosedTime <= DateTime.ParseExact(model.ToRegisterClosedTime, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1));
            }
            if (model.FromDate != null)
            {
                query = query.Where(c => c.OpenTime >= DateTime.ParseExact(model.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            if (model.ToRegisterClosedTime != null)
            {
                query = query.Where(c => c.OpenTime <= DateTime.ParseExact(model.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1));
            }
            query = query.OrderByDynamic<AuctionProductSort>("Id", LinqExtensions.Order.Desc);
            if (model.sortBy != null && model.sortType != null)
            {
                var sortDirection = LinqExtensions.Order.Asc;
                if (model.sortType == "DESC")
                {
                    sortDirection = LinqExtensions.Order.Desc;
                }
                query = query.OrderByDynamic<AuctionProductSort>(model.sortBy, sortDirection);
            }

            return new AuctionProductPaging()
            {
                data = await query.Skip(offSet).Take(model.pageSize).ToListAsync(),
                pageIndex = model.pageIndex,
                pageSize = model.pageSize,
                totalPage = query.Count() % model.pageSize > 0 ? query.Count() / model.pageSize + 1 : query.Count() / model.pageSize,
            };
        }
        public async Task<DTResult<AuctionProductViewModel>> ListServerSide(AuctionProductDTParameters parameters)
        {
            //0. Options
            try
            {
                string searchAll = parameters.SearchAll.Trim();//Trim text
                string orderCritirea = "Id";//Set default critirea
                int recordTotal, recordFiltered;
                bool orderDirectionASC = true;//Set default ascending
                if (parameters.Order != null)
                {
                    orderCritirea = parameters.Columns[parameters.Order[0].Column].Data;
                    orderDirectionASC = parameters.Order[0].Dir == DTOrderDir.ASC;
                }
                //1. Join
                var query = from row in db.AuctionProducts
                            join p in db.Products on row.ProductId equals p.Id
                            join o in db.Orders.Where(c => c.Active == 1) on row.Id equals o.AuctionProductId into go
                            from o in go.DefaultIfEmpty()
                            where row.Active == 1
                            && p.Active == 1
                            select new
                            {
                                row,
                                p,
                                o
                            };

                recordTotal = await query.CountAsync();
                //2. Fillter
                bool flagFillter = true;
                if (!String.IsNullOrEmpty(searchAll))
                {
                    flagFillter = false;
                    searchAll = searchAll.ToLower();
                    try
                    {
                        query = query.Where(c =>
                      EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //EF.Functions.Collate(c.row.FinalPrice.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //EF.Functions.Collate(c.row.RegisterOpenTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //
                    EF.Functions.Collate(c.row.RegisterClosedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.OpenTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //
                    //EF.Functions.Collate(c.row.ClosedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //EF.Functions.Collate(c.row.Code.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //EF.Functions.Collate(c.row.Description.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //EF.Functions.Collate(c.row.Active.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    //
                    EF.Functions.Collate(c.p.Name.ToLower().Replace("-", "").Replace(".", ""), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll.Replace("-", "").Replace(".", "").ToLower(), SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.p.Province.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll.ToLower(), SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.p.ProductCategory.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll.ToLower(), SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.p.ProductBrand.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll.ToLower(), SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.AuctionProductStatus.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll.ToLower(), SQLParams.Latin_General))

                  //EF.Functions.Collate(c.row.Search.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                  //
                  //EF.Functions.Collate(c.row.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))

                  );
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                foreach (var item in parameters.Columns)
                {
                    var fillter = item.Search.Value.Trim();
                    if (fillter.Length > 0)
                    {
                        switch (item.Data)
                        {
                            case "id":
                                query = query.Where(c => c.row.Id.ToString().Trim().Contains(fillter));
                                break;
                            case "productId":
                                var filter = fillter.Trim().Replace(".", "").Replace("-", "").ToLower();
                                if (filter.Contains("*"))
                                {
                                    var headCharacter = filter.Substring(0, filter.IndexOf("*"));
                                    var tailCharacter = filter.Substring(filter.IndexOf("*") + 1);
                                    string partern = $"{headCharacter}%{tailCharacter}";
                                    query = query.Where(c => EF.Functions.Like(c.p.Name.Trim().Replace(".", "").Replace("-", "").ToLower(), partern));
                                }
                                else
                                {
                                    query = query.Where(c => c.p.Name.Trim().Replace(".", "").Replace("-", "").ToLower().Contains(filter));
                                }
                                flagFillter = false;
                                break;
                            //case "finalPrice":
                            //    query = query.Where(c => c.row.FinalPrice.ToString().Trim().Contains(fillter));
                            //    break;
                            //case "registerOpenTime":
                            //    if (fillter.Contains(" - "))
                            //    {
                            //        var dates = fillter.Split(" - ");
                            //        var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //        var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                            //        query = query.Where(c => c.row.RegisterOpenTime >= startDate && c.row.RegisterOpenTime <= endDate);
                            //    }
                            //    else
                            //    {
                            //        var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //        query = query.Where(c => c.row.RegisterOpenTime.Date == date.Date);
                            //    }
                            //    break;
                            case "registerClosedTime":
                                if (fillter.Contains(" - "))
                                {
                                    var dates = fillter.Split(" - ");
                                    var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                    query = query.Where(c => c.row.RegisterClosedTime >= startDate && c.row.RegisterClosedTime <= endDate);
                                }
                                else
                                {
                                    var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var endDate = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                    query = query.Where(c => c.row.RegisterClosedTime >= date && c.row.RegisterClosedTime <= endDate);
                                }
                                flagFillter = false;
                                break;
                            case "openTime":
                                if (fillter.Contains(" - "))
                                {
                                    var dates = fillter.Split(" - ");
                                    var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                    query = query.Where(c => c.row.OpenTime >= startDate && c.row.OpenTime <= endDate);
                                }
                                else
                                {
                                    var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    var endDate = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                    query = query.Where(c => c.row.OpenTime >= date && c.row.OpenTime <= endDate);
                                }
                                flagFillter = false;
                                break;
                            //case "closedTime":
                            //    if (fillter.Contains(" - "))
                            //    {
                            //        var dates = fillter.Split(" - ");
                            //        var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //        var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                            //        query = query.Where(c => c.row.ClosedTime >= startDate && c.row.ClosedTime <= endDate);
                            //    }
                            //    else
                            //    {
                            //        var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            //        query = query.Where(c => c.row.ClosedTime.Date == date.Date);
                            //    }
                            //    break;
                            case "auctionProductBrand":
                                int data;
                                if (int.TryParse(fillter, out data))
                                {
                                    if (data > 0)
                                    {
                                        query = query.Where(x => x.p.ProductBrandId == data);
                                        flagFillter = false;
                                    }
                                    break;
                                }
                                else
                                {
                                    break;
                                }

                            case "auctionProductCategory":
                                var datafillter = fillter.Split(',');
                                if (datafillter.Count() > 1)
                                {
                                    query = query.Where(x => datafillter.Contains(x.p.ProductCategoryId.ToString()));
                                    flagFillter = false;
                                    break;
                                }
                                else
                                {
                                    query = query.Where(x => x.p.ProductCategoryId == int.Parse(fillter));
                                    flagFillter = false;
                                    break;
                                }

                            case "auctionProductStatusId":
                                var dataStatus = fillter.Split(',');
                                if (dataStatus.Count() > 1)
                                {
                                    query = query.Where(x => dataStatus.Contains(x.row.AuctionProductStatusId.ToString()));
                                    flagFillter = false;
                                    break;
                                }
                                else
                                {
                                    query = query.Where(x => x.row.AuctionProductStatusId == int.Parse(fillter));
                                    flagFillter = false;
                                    break;
                                }
                            case "productProvince":
                                int provinceId;
                                if (int.TryParse(fillter, out provinceId))
                                    if (provinceId > 0)
                                    {
                                        query = query.Where(x => x.p.ProvinceId == provinceId);
                                        flagFillter = false;
                                    }

                                break;
                                //case "code":
                                //    query = query.Where(c => (c.row.Code ?? "").Contains(fillter));
                                //    break;
                                //case "description":
                                //    query = query.Where(c => (c.row.Description ?? "").Contains(fillter));
                                //    break;
                                //case "active":
                                //    query = query.Where(c => c.row.Active.ToString().Trim().Contains(fillter));
                                //    break;
                                //case "search":
                                //    query = query.Where(c => (c.row.Search ?? "").Contains(fillter));
                                //    break;
                                //case "createdTime":
                                //    if (fillter.Contains(" - "))
                                //    {
                                //        var dates = fillter.Split(" - ");
                                //        var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                //        var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                //        query = query.Where(c => c.row.CreatedTime >= startDate && c.row.CreatedTime <= endDate);
                                //    }
                                //    else
                                //    {
                                //        var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                //        query = query.Where(c => c.row.CreatedTime.Date == date.Date);
                                //    }
                                //    break;

                        }
                    }
                }

                if (parameters.ProductIds.Count > 0)
                {
                    query = query.Where(c => parameters.ProductIds.Contains(c.row.Product.Id));
                    flagFillter = false;
                }


                if (parameters.AuctionProductIds.Count > 0)
                {
                    query = query.Where(c => parameters.AuctionProductIds.Contains(c.row.Product.Id));
                    flagFillter = false;
                }


                //3.Query second
                var query2 = query.Select(c => new AuctionProductViewModel()
                {
                    Id = c.row.Id,
                    ProductId = c.p.Id,
                    ProductName = c.p.Name,
                    FinalPrice = c.row.FinalPrice,
                    RegisterOpenTime = c.row.RegisterOpenTime,
                    RegisterClosedTime = c.row.RegisterClosedTime,
                    OpenTime = c.row.OpenTime,
                    ClosedTime = c.row.ClosedTime,
                    Code = c.row.Code,
                    Description = c.row.Description,
                    Active = c.row.Active,
                    Search = c.row.Search,
                    CreatedTime = c.row.CreatedTime,
                    ProductProvince = c.p.Province.Name,
                    ProvinveId = c.p.Province.Id,
                    AuctionProductBrand = c.p.ProductBrand.Name,
                    AuctionProductCategory = c.p.ProductCategory.Name,
                    AuctionProductCategoryId = c.p.ProductCategory.Id,
                    AuctionProductBrandId = c.p.ProductBrandId,
                    AuctionProductStatusName = c.row.AuctionProductStatus.Name,
                    AuctionProductStatusId = c.row.AuctionProductStatusId,
                    flagOrdered = ((c.o ?? new Order()).Id != 0) ? 1 : 0,
                    orderId = (c.o ?? new Order()).Id
                });

                //4. Sort
                query2 = query2.OrderByDynamic<AuctionProductViewModel>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
                //4.1 Custom order by province Sort by register closed time
                if (flagFillter)
                {
                    DateTime today = DateTime.Today;
                    query2 = query2.OrderByDescending(c => c.RegisterClosedTime >= today)
                            .ThenByDescending(x => x.ProvinveId == 1 && x.RegisterClosedTime >= today)
                            .ThenByDescending(x => x.ProvinveId == 2 && x.RegisterClosedTime >= today)
                            .ThenByDescending(x => x.ProvinveId == 44 && x.RegisterClosedTime >= today)
                            .ThenByDescending(x => x.ProvinveId == 4 && x.RegisterClosedTime >= today);
                }

                recordFiltered = await query2.CountAsync();
                //5. Return data
                return new DTResult<AuctionProductViewModel>()
                {
                    data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                    draw = parameters.Draw,
                    recordsFiltered = recordFiltered,
                    recordsTotal = recordTotal
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        
        }
        public DatabaseFacade GetDatabase()
        {
            return db.Database;
        }

        public async Task<List<AuctionProductViewModel>> detail2(int? id)
        {
            return await (
                from ap in db.AuctionProducts
                join p in db.Products on ap.ProductId equals p.Id
                join pp in db.Provinces on p.ProvinceId equals pp.Id
                join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                where ap.Active == 1 && pp.Active == 1 && pb.Active == 1 && pc.Active == 1 && p.Active == 1 && ap.Id == id
                select new AuctionProductViewModel
                {
                    Id = ap.Id,
                    FinalPrice = ap.FinalPrice,
                    ProductProvince = pp.Name,
                    AuctionProductBrand = pb.Name,
                    AuctionProductCategory = pc.Name,
                    RegisterOpenTime = ap.RegisterOpenTime,
                    RegisterClosedTime = ap.RegisterClosedTime,
                    OpenTime = ap.OpenTime,
                    ClosedTime = ap.ClosedTime,
                    ProductName = p.Name,
                    AuctionProductStatusId = ap.AuctionProductStatusId,
                    Description = ap.Description
                }
                ).ToListAsync();
        }
    }
}


