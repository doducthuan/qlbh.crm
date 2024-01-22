
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
using QLBH_Dion.Models.ViewModel;
using QLBH_Dion.Constants;
using Microsoft.EntityFrameworkCore.Infrastructure;
using static iText.IO.Image.Jpeg2000ImageData;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient.Server;

namespace QLBH_Dion.Repository
{
    public class OrdersRepository : IOrderRepository
    {
        QLBHContext db;
        public OrdersRepository(QLBHContext _db)
        {
            db = _db;
        }


        public async Task<List<Order>> List()
        {
            if (db != null)
            {
                return await (
                    from row in db.Orders
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }

            return null;
        }


        public async Task<List<Order>> Search(string keyword)
        {
            if (db != null)
            {
                return await (
                    from row in db.Orders
                    where (row.Active == 1 && (row.Id.ToString().Contains(keyword) || row.Description.Contains(keyword)))
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }
            return null;
        }


        public async Task<List<Order>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await (
                    from row in db.Orders
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }

        public async Task<OrderPaging> ListPagingSort(OrderPagingRequest model)
        {
            int offSet = 0;
            offSet = (model.pageIndex - 1) * model.pageSize;
            //1. Join
            var query = from row in db.Orders
                        join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                        join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                        join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                        join a in db.Accounts on row.AccountId equals a.Id
                        join pro in db.Products on ap.ProductId equals pro.Id
                        join province in db.Provinces on pro.ProvinceId equals province.Id
                        join proc in db.ProductCategories on pro.ProductCategoryId equals proc.Id
                        join prob in db.ProductBrands on pro.ProductBrandId equals prob.Id
                        where row.Active == 1 && ap.Active == 1 && ot.Active == 1 && os.Active == 1 && a.Active == 1 && pro.Active == 1 && province.Active == 1 && proc.Active == 1 && prob.Active == 1
                        orderby row.Id descending
                        select new OrderSort
                        {
                            Id = row.Id,
                            orderStatusId = row.OrderStatusId,
                            orderStatusName = os.Name,
                            Note = row.Note == null ? "" : row.Note,
                            ProvinceId = province.Id,
                            ProvinceName = province.Name,
                            ProductName = pro.Name,
                            ProductCategoryId = proc.Id,
                            ProductBrandId = prob.Id,
                            ProductCategoryName = proc.Name,
                            ProductBrandName = prob.Name,
                            CreatedTime = row.CreatedTime,
                            BuyTime = ap.OpenTime,
                            OrderPrice = row.Price == null ? 0 : row.Price,
                            FinalPrice = row.FinalPrice == null ? 0 : row.FinalPrice,
                        };
            //Lấy ra all data
            //2. Fillter

            if (!String.IsNullOrEmpty(model.keyword))
            {
                model.keyword = model.keyword.Replace("-","").Replace(".","").ToLower();
                try
                {
                    query = query.Where(c =>
                    EF.Functions.Collate(c.ProductName.Replace("-","").Replace(".","").ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(model.keyword, SQLParams.Latin_General))
                );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            if (model.OrderStatusId != null)
            {
                query = query.Where(c => c.orderStatusId == model.OrderStatusId);
            }
            if (model.ProvinceId != null)
            {
                query = query.Where(c => c.ProvinceId == model.ProvinceId);
            }
            if (model.ProductCategoryId != null)
            {
                query = query.Where(c => c.ProductCategoryId == model.ProductCategoryId);
            }
            if (model.ProductBrandId != null)
            {
                query = query.Where(c => c.ProductBrandId == model.ProductBrandId);
            }
            // Lọc theo từ ngày đến ngày
            if (model.FromDate != null)
            {
                query = query.Where(c => c.BuyTime >= DateTime.ParseExact(model.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            if (model.ToDate != null)
            {
                query = query.Where(c => c.BuyTime <= DateTime.ParseExact(model.ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1));
            }
            query = query.OrderByDynamic<OrderSort>("Id", LinqExtensions.Order.Desc);
            if (model.sortBy != null && model.sortType != null)
            {
                var sortDirection = LinqExtensions.Order.Asc;
                if (model.sortType == "DESC")
                {
                    sortDirection = LinqExtensions.Order.Desc;
                }
                query = query.OrderByDynamic<OrderSort>(model.sortBy, sortDirection);
            }

            return new OrderPaging()
            {
                data = await query.Skip(offSet).Take(model.pageSize).ToListAsync(),
                pageIndex = model.pageIndex,
                pageSize = model.pageSize,
                totalPage = query.Count() % model.pageSize > 0 ? query.Count() / model.pageSize + 1 : query.Count() / model.pageSize,
            };
        }
        public async Task<List<OrderSort>> GetAllProductDetail()
        {
            if(db != null)
            {
                return await (from row in db.Orders
                              join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                              join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                              join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                              join a in db.Accounts on row.AccountId equals a.Id
                              join pro in db.Products on ap.ProductId equals pro.Id
                              join province in db.Provinces on pro.ProvinceId equals province.Id
                              join proc in db.ProductCategories on pro.ProductCategoryId equals proc.Id
                              join prob in db.ProductBrands on pro.ProductBrandId equals prob.Id
                              where row.Active == 1 && ap.Active == 1 && ot.Active == 1 && os.Active == 1 && a.Active == 1 && pro.Active == 1 && province.Active == 1 && proc.Active == 1 && prob.Active == 1
                              orderby row.Id descending
                              select new OrderSort
                              {
                                  Id = row.Id,
                                  orderStatusId = row.OrderStatusId,
                                  orderStatusName = os.Name,
                                  Note = row.Note == null ? "" : row.Note,
                                  ProvinceId = province.Id,
                                  ProvinceName = province.Name,
                                  ProductName = pro.Name,
                                  ProductCategoryId = proc.Id,
                                  ProductBrandId = prob.Id,
                                  ProductCategoryName = proc.Name,
                                  ProductBrandName = prob.Name,
                                  CreatedTime = row.CreatedTime,
                                  BuyTime = ap.OpenTime,
                                  OrderPrice = row.Price == null ? 0 : row.Price,
                              }).ToListAsync();
            }
            return null;
        }
        public async Task<OrderSort> GetDetailOderSort(int id)
        {
            if(db != null)
            {
                var query = await (from row in db.Orders
                              join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                              join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                              join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                              join a in db.Accounts on row.AccountId equals a.Id
                              join pro in db.Products on ap.ProductId equals pro.Id
                              join province in db.Provinces on pro.ProvinceId equals province.Id
                              join proc in db.ProductCategories on pro.ProductCategoryId equals proc.Id
                              join prob in db.ProductBrands on pro.ProductBrandId equals prob.Id
                              where row.Active == 1 && ap.Active == 1 && ot.Active == 1 && os.Active == 1 && a.Active == 1 && pro.Active == 1 && province.Active == 1 && proc.Active == 1 && prob.Active == 1 && row.Id == id
                              orderby row.Id descending
                              select new OrderSort
                              {
                                  Id = row.Id,
                                  orderStatusId = row.OrderStatusId,
                                  orderStatusName = os.Name,
                                  Note = row.Note == null ? "" : row.Note,
                                  ProvinceId = province.Id,
                                  ProvinceName = province.Name,
                                  ProductName = pro.Name,
                                  ProductCategoryId = proc.Id,
                                  ProductBrandId = prob.Id,
                                  ProductCategoryName = proc.Name,
                                  ProductBrandName = prob.Name,
                                  CreatedTime = row.CreatedTime,
                                  BuyTime = ap.OpenTime,
                                  OrderPrice = row.Price == null ? 0 : row.Price,
                                  FinalPrice = row.FinalPrice == null ? 0 : row.FinalPrice
                              }).FirstOrDefaultAsync();
                return query;
            }
            return null;
        }
        public async Task<List<Order>> Detail(int? id)
        {
            if (db != null)
            {
                return await (
                    from row in db.Orders
                    where (row.Active == 1 && row.Id == id)
                    select row)
                    .ToListAsync();
            }

            return null;
        }
        public async Task<OrdersViewModel> Detail2(int orderId)
        {
              
            if(db != null)
            {             
                var query = await (from row in db.Orders
                                   join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                                   join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                                   join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                                   join a in db.Accounts on row.AccountId equals a.Id
                                   join p in db.Products on ap.ProductId equals p.Id
                                   join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                                   join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                                   join pv in db.Provinces on p.ProvinceId equals pv.Id
                                   where row.Active == 1 && ap.Active == 1 && os.Active == 1 && a.Active == 1 && row.Id ==orderId
                                   && p.Active == 1 && pc.Active == 1 && pb.Active == 1 && pv.Active == 1
                                   orderby row.Id descending
                                   select new OrdersViewModel
                                   {
                                       Id = row.Id,
                                       ProductName = p.Name,
                                       RegisterOpenTime = ap.RegisterOpenTime,
                                       RegisterClosedTime = ap.RegisterClosedTime,
                                       OpenTime = ap.OpenTime,
                                       ClosedTime = ap.ClosedTime,
                                       ProductBrandName = pb.Name,
                                       ProductCategoryName = pc.Name,
                                       ProvinceName = pv.Name,                                    
                                       ListAccountBuy = (from oa in db.OrderAccounts
                                                         join ab in db.AccountBuys on oa.AccountBuyId equals ab.Id
                                                         where oa.Active == 1 && ab.Active == 1 && row.Id == oa.OrderId
                                                         select new AccountBuy
                                                         {
                                                             Username = ab.Username,
                                                             Password = ab.Password,
                                                             Description = oa.Description,
                                                         }).ToList(),
                                       OrderStatusId = row.OrderStatusId,
                                       OrderStatusName = os.Name,
                                       Price = row.Price,
                                       FinalPrice = row.FinalPrice,
                                       OrderStatusColor = os.Color,
                                       AccountName = a.FullName,
                                       CreatedTime = row.CreatedTime,
                                       Note = row.Note
                                   }).FirstOrDefaultAsync();
                return query;
            }
            return null;
        }


        public async Task<Order> Add(Order obj)
        {
            if (db != null)
            {
                await db.Orders.AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            return null;
        }


        public async Task Update(Order obj)
        {
            if (db != null)
            {
                //Update that object
                db.Orders.Attach(obj);
                db.Entry(obj).Property(x => x.AuctionProductId).IsModified = true;
                db.Entry(obj).Property(x => x.OrderTypeId).IsModified = true;
                db.Entry(obj).Property(x => x.OrderStatusId).IsModified = true;
                db.Entry(obj).Property(x => x.AccountId).IsModified = true;
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                db.Entry(obj).Property(x => x.Active).IsModified = true;
                db.Entry(obj).Property(x => x.Search).IsModified = true;
                db.Entry(obj).Property(x => x.Note).IsModified = true;
                db.Entry(obj).Property(x => x.Note2).IsModified = true;
                db.Entry(obj).Property(x => x.Note3).IsModified = true;
                db.Entry(obj).Property(x => x.Note4).IsModified = true;
                db.Entry(obj).Property(x => x.Note5).IsModified = true;
                db.Entry(obj).Property(x => x.Price).IsModified = true;
                db.Entry(obj).Property(x => x.FinalPrice).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateOrderSort(UpdateOrder obj)
        {
            if (db != null)
            {
                //Update that object
                var orderobj = db.Orders.FirstOrDefault(x => x.Id == obj.Id);
                orderobj.OrderStatusId = obj.OrderStatusId;
                if (obj.Note != null)
                {
                    orderobj.Note = obj.Note;
                }
                if (obj.Price != null)
                {
                    orderobj.Price = obj.Price;
                }
                db.Entry(orderobj).Property(x => x.OrderStatusId).IsModified = true;
                db.Entry(orderobj).Property(x => x.Price).IsModified = true;
                db.Entry(orderobj).Property(x => x.Note).IsModified = true;

                //Commit the transaction
                return await db.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task UpdateByViewModel(Order obj)
        {
            if (db != null)
            {
                db.Orders.Attach(obj);
                db.Entry(obj).Property(x => x.OrderStatusId).IsModified = true;
                db.Entry(obj).Property(x => x.Note).IsModified = true;
                //db.Entry(obj).Property(x => x.Note2).IsModified = true;
                //db.Entry(obj).Property(x => x.Note3).IsModified = true;
                //db.Entry(obj).Property(x => x.Note4).IsModified = true;
                //db.Entry(obj).Property(x => x.Note5).IsModified = true;
                db.Entry(obj).Property(x => x.Price).IsModified = true;
                db.Entry(obj).Property(x => x.FinalPrice).IsModified = true;
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(Order obj)
        {
            if (db != null)
            {
                //Update that obj
                db.Orders.Attach(obj);
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
                var obj = await db.Orders.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.Orders.Remove(obj);

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
                    from row in db.Orders
                    where row.Active == 1
                    select row
                            ).Count();
            }

            return result;
        }
        public async Task<List<Order>> GetTotalOrderStatus()
        {
            if (db != null)
            {
                return await (from row in db.Orders
                              where row.Active == 1 &&
                              (row.OrderStatusId == OrderStatusId.NOPROCESS ||
                               row.OrderStatusId == OrderStatusId.APPROVED ||
                               row.OrderStatusId == OrderStatusId.DEPOSITED ||
                               row.OrderStatusId == OrderStatusId.PURCHAED ||
                               row.OrderStatusId == OrderStatusId.CANCEL ||
                               row.OrderStatusId == OrderStatusId.NOPURCHAED)
                              select row).ToListAsync();              
            }
            return null;
        }
        public async Task<DTResult<OrdersViewModel>> ListServerSide(OrdersDTParameters parameters, int accountId, int accountRoleId)
        {
            //0. Options

            string searchAll = parameters.SearchAll.Trim().ToLower();
            string orderCritirea = "Id";//Set default critirea
            int recordTotal, recordFiltered;
            bool orderDirectionASC = true;//Set default ascending
            if (parameters.Order != null)
            {
                orderCritirea = parameters.Columns[parameters.Order[0].Column].Data;
                orderDirectionASC = parameters.Order[0].Dir == DTOrderDir.ASC;
            }
            //1. Join
            var query = GetDataByConditions(accountId, accountRoleId);

            recordTotal = await query.CountAsync();
            //2. Fillter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.Replace("-", "").Replace(".", "").Replace("#","").ToLower();
                try
                {
                    query = query.Where(c =>
                    EF.Functions.Collate(c.Id.ToString().Replace("#", "").ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.Price.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.ProductName.Replace("-", "").Replace(".", "").ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.Note.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.OrderStatusName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.AccountName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))||
                    EF.Functions.Collate(c.ClosedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.OpenTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.RegisterClosedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.RegisterOpenTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))
                    );
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }
            foreach (var item in parameters.Columns)
            {
                var fillter = item.Search.Value.ToString().ToLower().Trim();
                if (fillter.Length > 0)
                {
                    switch (item.Data)
                    {
                        case "id":
                            query = query.Where(c => c.Id.ToString().Contains(fillter.Replace("#", "")));
                            break;
                        case "productName":

                            query = query.Where(c => c.ProductName.Replace("-","").Replace(".","").ToLower().Contains(fillter.Replace("-", "").Replace(".", "")));
                            break;
                        case "price":
                            var searchMoney = fillter.Split('-');
                            if (Convert.ToInt64(searchMoney[0]) == 1000000000)
                            {
                                query = query.Where(c => c.Price > 1000000000);
                            }
                            else
                            {
                                query = query.Where(c => c.Price >= Convert.ToInt64(searchMoney[0]) && c.Price <= Convert.ToInt64(searchMoney[1]));
                            }                            
                            break;
                        case "registerClosedTime":
                            var searchDateArrsRegister = fillter.Split(',');
                            if(searchDateArrsRegister.Length == 3)
                            {                               
                                query = query.Where(c => c.RegisterClosedTime <= Convert.ToDateTime(searchDateArrsRegister[1]) && c.RegisterClosedTime >= Convert.ToDateTime(searchDateArrsRegister[0]));
                            }
                            else
                            {
                                query = query.Where(c => c.RegisterClosedTime >= Convert.ToDateTime(searchDateArrsRegister[0]) && c.RegisterClosedTime <= Convert.ToDateTime(searchDateArrsRegister[1]));
                            }
                            
                            break;
                        case "closedTime":
                            var searchDateArrsAuction = fillter.Split(',');

                            if (searchDateArrsAuction.Length == 3)
                            {
                                query = query.Where(c => c.ClosedTime <= Convert.ToDateTime(searchDateArrsAuction[1]) && c.ClosedTime >= Convert.ToDateTime(searchDateArrsAuction[0]));
                            }
                            else
                            {
                                query = query.Where(c => c.OpenTime >= Convert.ToDateTime(searchDateArrsAuction[0]) && c.OpenTime <= Convert.ToDateTime(searchDateArrsAuction[1]));
                            }
                            break;
                        case "orderStatusId":
                            query = query.Where(c => fillter.Contains(c.OrderStatusId.ToString()));
                            break;

                        case "accountName":
                            query = query.Where(c => EF.Functions.Collate(c.AccountName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter, SQLParams.Latin_General)));
                            break;
                        case "createdTime":
                            var searchDateArrs = fillter.Split(',');
                            if(searchDateArrs.Length == 3)
                            {
                                query = query.Where(c => c.CreatedTime <= Convert.ToDateTime(searchDateArrs[1]) && c.CreatedTime >= Convert.ToDateTime(searchDateArrs[0]));
                            }
                            else
                            {
                                query = query.Where(c => c.CreatedTime >= Convert.ToDateTime(searchDateArrs[0]) && c.CreatedTime <= Convert.ToDateTime(searchDateArrs[1]));
                            }
                            
                            break;
                        default:
                            break;
                    }
                }
            }
            var query2 = query.Select(c => new OrdersViewModel
            {
                Id = c.Id,
                ProductName = c.ProductName,
                AuctionProduct = c.AuctionProduct,
                ListAccountBuy = c.ListAccountBuy,
                OrderStatusId = c.OrderStatusId,
                OrderStatusName = c.OrderStatusName,
                OrderStatusColor = c.OrderStatusColor,
                Price = c.Price,
                AccountName = c.AccountName,
                CreatedTime = c.CreatedTime,
                RegisterOpenTime = c.RegisterOpenTime,
                RegisterClosedTime = c.RegisterClosedTime,
                OpenTime = c.OpenTime,
                ClosedTime = c.ClosedTime,
                ProductBrandName = c.ProductBrandName,
                ProductCategoryName = c.ProductCategoryName,
                ProvinceName = c.ProvinceName
            });
            //4. Sort
            query2 = query2.OrderByDynamic<OrdersViewModel>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<OrdersViewModel>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
        public IQueryable<OrdersViewModel> GetDataByConditions(int accountId, int accountRoleId)
        {
            if(accountRoleId == RoleId.CONSUMER)
            {
                return      from row in db.Orders
                            join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                            join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                            join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                            join a in db.Accounts on row.AccountId equals a.Id
                            join p in db.Products on ap.ProductId equals p.Id
                            join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                            join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                            join pv in db.Provinces on p.ProvinceId equals pv.Id
                            where row.Active == 1 && ap.Active == 1 && os.Active == 1 && a.Active == 1
                            && p.Active == 1 && pc.Active == 1 && pb.Active == 1 && pv.Active == 1 && row.AccountId == accountId
                            orderby row.Id descending
                            select new OrdersViewModel
                            {
                                Id = row.Id,
                                ProductName = p.Name,
                                RegisterOpenTime = ap.RegisterOpenTime,
                                RegisterClosedTime = ap.RegisterClosedTime,
                                OpenTime = ap.OpenTime,
                                ClosedTime = ap.ClosedTime,
                                ProductBrandName = pb.Name,
                                ProductCategoryName = pc.Name,
                                ProvinceName = pv.Name,
                                ListAccountBuy = (from oa in db.OrderAccounts
                                                  join ab in db.AccountBuys on oa.AccountBuyId equals ab.Id
                                                  where oa.Active == 1 && ab.Active == 1 && row.Id == oa.OrderId
                                                  select new AccountBuy
                                                  {
                                                      Username = ab.Username,
                                                      Password = ab.Password,
                                                      Description = oa.Description,
                                                  }).ToList(),
                                OrderStatusId = row.OrderStatusId,
                                OrderStatusName = os.Name,
                                Price = row.Price,
                                OrderStatusColor = os.Color,
                                AccountName = a.FullName,
                                CreatedTime = row.CreatedTime,
                                Note = row.Note
                            };
            }
            else if(accountRoleId == RoleId.ACCOUNTANT)
            {
                return      from row in db.Orders
                            join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                            join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                            join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                            join a in db.Accounts on row.AccountId equals a.Id
                            join p in db.Products on ap.ProductId equals p.Id
                            join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                            join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                            join pv in db.Provinces on p.ProvinceId equals pv.Id
                            where row.Active == 1 && ap.Active == 1 && os.Active == 1 && a.Active == 1
                            && p.Active == 1 && pc.Active == 1 && pb.Active == 1 && pv.Active == 1 && (row.OrderStatusId == OrderStatusId.APPROVED || row.OrderStatusId == OrderStatusId.DEPOSITED || row.OrderStatusId == OrderStatusId.PURCHAED)
                            orderby row.Id descending
                            select new OrdersViewModel
                            {
                                Id = row.Id,
                                ProductName = p.Name,
                                RegisterOpenTime = ap.RegisterOpenTime,
                                RegisterClosedTime = ap.RegisterClosedTime,
                                OpenTime = ap.OpenTime,
                                ClosedTime = ap.ClosedTime,
                                ProductBrandName = pb.Name,
                                ProductCategoryName = pc.Name,
                                ProvinceName = pv.Name,
                                ListAccountBuy = (from oa in db.OrderAccounts
                                                  join ab in db.AccountBuys on oa.AccountBuyId equals ab.Id
                                                  where oa.Active == 1 && ab.Active == 1 && row.Id == oa.OrderId
                                                  select new AccountBuy
                                                  {
                                                      Username = ab.Username,
                                                      Password = ab.Password,
                                                      Description = oa.Description,
                                                  }).ToList(),
                                OrderStatusId = row.OrderStatusId,
                                OrderStatusName = os.Name,
                                Price = row.Price,
                                OrderStatusColor = os.Color,
                                AccountName = a.FullName,
                                CreatedTime = row.CreatedTime,
                                Note = row.Note
                            };
            }
            else if(accountRoleId == RoleId.PURCHASING_EMPLOYEE)
            {
                return      from row in db.Orders
                            join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                            join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                            join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                            join a in db.Accounts on row.AccountId equals a.Id
                            join p in db.Products on ap.ProductId equals p.Id
                            join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                            join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                            join pv in db.Provinces on p.ProvinceId equals pv.Id
                            where row.Active == 1 && ap.Active == 1 && os.Active == 1 && a.Active == 1
                            && p.Active == 1 && pc.Active == 1 && pb.Active == 1 && pv.Active == 1 && (row.OrderStatusId == OrderStatusId.DEPOSITED || row.OrderStatusId == OrderStatusId.PURCHAED || row.OrderStatusId == OrderStatusId.APPROVED || row.OrderStatusId == OrderStatusId.NOPURCHAED)
                            orderby row.Id descending
                            select new OrdersViewModel
                            {
                                Id = row.Id,
                                ProductName = p.Name,
                                RegisterOpenTime = ap.RegisterOpenTime,
                                RegisterClosedTime = ap.RegisterClosedTime,
                                OpenTime = ap.OpenTime,
                                ClosedTime = ap.ClosedTime,
                                ProductBrandName = pb.Name,
                                ProductCategoryName = pc.Name,
                                ProvinceName = pv.Name,
                                ListAccountBuy = (from oa in db.OrderAccounts
                                                  join ab in db.AccountBuys on oa.AccountBuyId equals ab.Id
                                                  where oa.Active == 1 && ab.Active == 1 && row.Id == oa.OrderId
                                                  select new AccountBuy
                                                  {
                                                      Username = ab.Username,
                                                      Password = ab.Password,
                                                      Description = oa.Description,
                                                  }).ToList(),
                                OrderStatusId = row.OrderStatusId,
                                OrderStatusName = os.Name,
                                Price = row.Price,
                                OrderStatusColor = os.Color,
                                AccountName = a.FullName,
                                CreatedTime = row.CreatedTime,
                                Note = row.Note
                            };
            }
            else if (accountRoleId == RoleId.ADMIN)
            {
                return from row in db.Orders
                       join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                       join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                       join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                       join a in db.Accounts on row.AccountId equals a.Id
                       join p in db.Products on ap.ProductId equals p.Id
                       join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                       join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                       join pv in db.Provinces on p.ProvinceId equals pv.Id
                       where row.Active == 1 && ap.Active == 1 && os.Active == 1 && a.Active == 1
                       && p.Active == 1 && pc.Active == 1 && pb.Active == 1 && pv.Active == 1
                       orderby row.Id descending
                       select new OrdersViewModel
                       {
                           Id = row.Id,
                           ProductName = p.Name,
                           RegisterOpenTime = ap.RegisterOpenTime,
                           RegisterClosedTime = ap.RegisterClosedTime,
                           OpenTime = ap.OpenTime,
                           ClosedTime = ap.ClosedTime,
                           ProductBrandName = pb.Name,
                           ProductCategoryName = pc.Name,
                           ProvinceName = pv.Name,
                           ListAccountBuy = (from oa in db.OrderAccounts
                                             join ab in db.AccountBuys on oa.AccountBuyId equals ab.Id
                                             where oa.Active == 1 && ab.Active == 1 && row.Id == oa.OrderId
                                             select new AccountBuy
                                             {
                                                 Username = ab.Username,
                                                 Password = ab.Password,
                                                 Description = oa.Description,
                                             }).ToList(),
                           OrderStatusId = row.OrderStatusId,
                           OrderStatusName = os.Name,
                           Price = row.Price,
                           OrderStatusColor = os.Color,
                           AccountName = a.FullName,
                           CreatedTime = row.CreatedTime,
                           Note = row.Note
                       };
            }
            else
            {
                return     from row in db.Orders
                            join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                            join ot in db.OrderTypes on row.OrderTypeId equals ot.Id
                            join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                            join a in db.Accounts on row.AccountId equals a.Id
                            join p in db.Products on ap.ProductId equals p.Id
                            join pc in db.ProductCategories on p.ProductCategoryId equals pc.Id
                            join pb in db.ProductBrands on p.ProductBrandId equals pb.Id
                            join pv in db.Provinces on p.ProvinceId equals pv.Id
                            where row.Active == 1 && ap.Active == 1 && os.Active == 1 && a.Active == 1
                            && p.Active == 1 && pc.Active == 1 && pb.Active == 1 && pv.Active == 1 && row.OrderStatusId != OrderStatusId.NOPURCHAED
                            orderby row.Id descending
                            select new OrdersViewModel
                            {
                                Id = row.Id,
                                ProductName = p.Name,
                                RegisterOpenTime = ap.RegisterOpenTime,
                                RegisterClosedTime = ap.RegisterClosedTime,
                                OpenTime = ap.OpenTime,
                                ClosedTime = ap.ClosedTime,
                                ProductBrandName = pb.Name,
                                ProductCategoryName = pc.Name,
                                ProvinceName = pv.Name,
                                ListAccountBuy = (from oa in db.OrderAccounts
                                                  join ab in db.AccountBuys on oa.AccountBuyId equals ab.Id
                                                  where oa.Active == 1 && ab.Active == 1 && row.Id == oa.OrderId
                                                  select new AccountBuy
                                                  {
                                                      Username = ab.Username,
                                                      Password = ab.Password,
                                                      Description = oa.Description,
                                                  }).ToList(),
                                OrderStatusId = row.OrderStatusId,
                                OrderStatusName = os.Name,
                                Price = row.Price,
                                OrderStatusColor = os.Color,
                                AccountName = a.FullName,
                                CreatedTime = row.CreatedTime,
                                Note = row.Note
                            };
            }
        }
        public async Task<List<OrdersViewModel>> ExportOrdersExcel(FillterExportOrderExcel fillter)
        {
            List<OrdersViewModel> listOrderViewModel = new List<OrdersViewModel>();
            var query = from row in db.Orders
                        join ap in db.AuctionProducts on row.AuctionProductId equals ap.Id
                        join os in db.OrderStatuses on row.OrderStatusId equals os.Id
                        join a in db.Accounts on row.AccountId equals a.Id
                        join p in db.Products on ap.ProductId equals p.Id
                        where row.Active == 1 && ap.Active == 1 && os.Active == 1 && a.Active == 1
                        && p.Active == 1
                        orderby row.Id descending
                        select new OrdersViewModel
                        {
                            Id = row.Id,
                            ProductName = p.Name,
                            RegisterOpenTime = ap.RegisterOpenTime,
                            RegisterClosedTime = ap.RegisterClosedTime,
                            OpenTime = ap.OpenTime,
                            ClosedTime = ap.ClosedTime,
                            OrderStatusId = row.OrderStatusId,
                            OrderStatusName = os.Name,
                            Price = row.Price,
                            AccountName = a.FullName,
                            CreatedTime = row.CreatedTime,
                        };
            if (fillter.Id != "")
            {
                query = query.Where(x => x.Id.ToString() == fillter.Id.Replace("#",""));
            }
            if (fillter.ProductName != "")
            {
                query = query.Where(x => x.ProductName.Replace("-", "").Replace(".", "").ToLower().Contains(fillter.ProductName.Replace("-", "").Replace(".", "")));
            }
            if (fillter.AccountName != "")
            {
                query = query.Where(x => EF.Functions.Collate(x.AccountName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter.AccountName, SQLParams.Latin_General)));
            }
            if (fillter.Price != "")
            {
                var searchMoney = fillter.Price.Split('-');
                if (Convert.ToInt64(searchMoney[0]) == 1000000000)
                {
                    query = query.Where(c => c.Price > 1000000000);
                }
                else
                {
                    query = query.Where(c => c.Price >= Convert.ToInt64(searchMoney[0]) && c.Price <= Convert.ToInt64(searchMoney[1]));
                }
            }
            if (fillter.RegisterClosedTime != "")
            {
                var searchDateArrsRegister = fillter.RegisterClosedTime.Split(',');
                if (searchDateArrsRegister.Length == 3)
                {
                    query = query.Where(c => c.RegisterClosedTime <= Convert.ToDateTime(searchDateArrsRegister[1]) && c.RegisterClosedTime >= Convert.ToDateTime(searchDateArrsRegister[0]));
                }
                else
                {
                    query = query.Where(c => c.RegisterClosedTime >= Convert.ToDateTime(searchDateArrsRegister[0]) && c.RegisterClosedTime <= Convert.ToDateTime(searchDateArrsRegister[1]));
                }
            }
            if (fillter.AuctionTime != "")
            {
                var searchDateArrsAuction = fillter.AuctionTime.Split(',');
                if (searchDateArrsAuction.Length == 3)
                {
                    query = query.Where(c => c.ClosedTime <= Convert.ToDateTime(searchDateArrsAuction[1]) && c.ClosedTime >= Convert.ToDateTime(searchDateArrsAuction[0]));
                }
                else
                {
                    query = query.Where(c => c.OpenTime >= Convert.ToDateTime(searchDateArrsAuction[0]) && c.OpenTime <= Convert.ToDateTime(searchDateArrsAuction[1]));
                }
            }
            if (fillter.OrderStatusId.Length > 0)
            {
                query = query.Where(c => fillter.OrderStatusId.Contains(c.OrderStatusId.ToString()));
            }
            if (fillter.CreatedTime != "")
            {
                var searchDateArrs = fillter.CreatedTime.Split(',');
                if (searchDateArrs.Length == 3)
                {
                    query = query.Where(c => c.CreatedTime <= Convert.ToDateTime(searchDateArrs[1]) && c.CreatedTime >= Convert.ToDateTime(searchDateArrs[0]));
                }
                else
                {
                    query = query.Where(c => c.CreatedTime >= Convert.ToDateTime(searchDateArrs[0]) && c.CreatedTime <= Convert.ToDateTime(searchDateArrs[1]));
                }
            }
            if (query != null)
            {
                listOrderViewModel = query.ToList();
            }
            return listOrderViewModel;
        }
        public async Task ChangeOrderStatus(Order model)
        {
            if(db != null)
            {
                db.Orders.Attach(model);
                db.Entry(model).Property(x => x.OrderStatusId).IsModified = true;
                await db.SaveChangesAsync();
            }
        }
      
        public async Task<Order> GetOrderStatusAndFinal(int orderId)
        {
            var query = await (from row in db.Orders
                               where row.Active == 1 && row.Id == orderId
                               select new Order
                               {
                                   OrderStatusId = row.OrderStatusId,
                                   FinalPrice = row.FinalPrice,
                                   Price = row.Price
                               }).FirstOrDefaultAsync();
            return query;
        }
        public DatabaseFacade GetDatabase()
        {
            return db.Database;
        }
    }

}



