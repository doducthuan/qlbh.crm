
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

namespace QLBH_Dion.Repository
{
    public class OrderAccountRepository : IOrderAccountRepository
    {
        QLBHContext db;
        public OrderAccountRepository(QLBHContext _db)
        {
            db = _db;
        }


        public async Task<List<OrderAccount>> List()
        {
            if (db != null)
            {
                return await (
                    from row in db.OrderAccounts
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }

            return null;
        }


        public async Task<List<OrderAccount>> Search(string keyword)
        {
            if (db != null)
            {
                return await (
                    from row in db.OrderAccounts
                    where (row.Active == 1 && (row.Name.Contains(keyword) || row.Description.Contains(keyword)))
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }
            return null;
        }


        public async Task<List<OrderAccount>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await (
                    from row in db.OrderAccounts
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }


        public async Task<List<OrderAccount>> Detail(int? id)
        {
            if (db != null)
            {
                return await (
                    from row in db.OrderAccounts
                    where (row.Active == 1 && row.Id == id)
                    select row)
                    .ToListAsync();
            }

            return null;
        }


        public async Task<OrderAccount> Add(OrderAccount obj)
        {
            if (db != null)
            {
                await db.OrderAccounts.AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            return null;
        }


        public async Task Update(OrderAccount obj)
        {
            if (db != null)
            {
                //Update that object
                db.OrderAccounts.Attach(obj);
                db.Entry(obj).Property(x => x.AccountBuyId).IsModified = true;
                db.Entry(obj).Property(x => x.OrderId).IsModified = true;
                db.Entry(obj).Property(x => x.OrderPaymentStatusId).IsModified = true;
                db.Entry(obj).Property(x => x.MaxPrice).IsModified = true;
                db.Entry(obj).Property(x => x.WinPrice).IsModified = true;
                db.Entry(obj).Property(x => x.OrderAccountStatusId).IsModified = true;
                db.Entry(obj).Property(x => x.Name).IsModified = true;
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                db.Entry(obj).Property(x => x.Active).IsModified = true;
                db.Entry(obj).Property(x => x.Search).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }
        
        public async Task UpdateByViewModel(OrderAccount obj)
        {
            if(db != null)
            {
                db.OrderAccounts.Attach(obj);
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                db.Entry(obj).Property(x => x.OrderPaymentStatusId).IsModified = true;
                db.Entry(obj).Property(x => x.OrderAccountStatusId).IsModified = true;
                await db.SaveChangesAsync();
            }
        }
        public async Task Delete(OrderAccount obj)
        {
            if (db != null)
            {
                //Update that obj
                db.OrderAccounts.Attach(obj);
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
                var obj = await db.OrderAccounts.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.OrderAccounts.Remove(obj);

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
                    from row in db.OrderAccounts
                    where row.Active == 1
                    select row
                            ).Count();
            }

            return result;
        }
        public async Task<DTResult<OrderAccountViewModel>> ListServerSide(OrderAccountDTParameters parameters)
        {
            //0. Options
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
            var query = from row in db.OrderAccounts

                        join ab in db.AccountBuys on row.AccountBuyId equals ab.Id
                        join oas in db.OrderAccountStatuses on row.OrderAccountStatusId equals oas.Id

                        where row.Active == 1
                                        && ab.Active == 1
    && oas.Active == 1

                        select new
                        {
                            row,
                            ab,
                            oas
                        };

            recordTotal = await query.CountAsync();
            //2. Fillter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c =>
                    EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.OrderId.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.OrderPaymentStatusId.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.MaxPrice.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.WinPrice.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Description.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Active.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Search.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))

                );
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
                        case "orderId":
                            query = query.Where(c => c.row.OrderId.ToString().Trim().Contains(fillter));
                            break;
                        case "orderPaymentStatusId":
                            query = query.Where(c => c.row.OrderPaymentStatusId.ToString().Trim().Contains(fillter));
                            break;
                        case "maxPrice":
                            query = query.Where(c => c.row.MaxPrice.ToString().Trim().Contains(fillter));
                            break;
                        case "minPrice":
                            query = query.Where(c => c.row.WinPrice.ToString().Trim().Contains(fillter));
                            break;
                        case "name":
                            query = query.Where(c => (c.row.Name ?? "").Contains(fillter));
                            break;
                        case "description":
                            query = query.Where(c => (c.row.Description ?? "").Contains(fillter));
                            break;
                        case "active":
                            query = query.Where(c => c.row.Active.ToString().Trim().Contains(fillter));
                            break;
                        case "search":
                            query = query.Where(c => (c.row.Search ?? "").Contains(fillter));
                            break;
                        case "createdTime":
                            if (fillter.Contains(" - "))
                            {
                                var dates = fillter.Split(" - ");
                                var startDate = DateTime.ParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                var endDate = DateTime.ParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddSeconds(-1);
                                query = query.Where(c => c.row.CreatedTime >= startDate && c.row.CreatedTime <= endDate);
                            }
                            else
                            {
                                var date = DateTime.ParseExact(fillter, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                query = query.Where(c => c.row.CreatedTime.Date == date.Date);
                            }
                            break;

                    }
                }
            }

            if (parameters.AccountBuyIds.Count > 0)
            {
                query = query.Where(c => parameters.AccountBuyIds.Contains(c.row.AccountBuy.Id));
            }


            if (parameters.OrderAccountStatusIds.Count > 0)
            {
                query = query.Where(c => parameters.OrderAccountStatusIds.Contains(c.row.OrderAccountStatus.Id));
            }


            //3.Query second
            var query2 = query.Select(c => new OrderAccountViewModel()
            {
                Id = c.row.Id,
                AccountBuyId = c.ab.Id,
                OrderId = c.row.OrderId,
                OrderPaymentStatusId = c.row.OrderPaymentStatusId,
                MaxPrice = c.row.MaxPrice,
                WinPrice = c.row.WinPrice,
                OrderAccountStatusId = c.oas.Id,
                OrderAccountStatusName = c.oas.Name,
                Name = c.row.Name,
                Description = c.row.Description,
                Active = c.row.Active,
                Search = c.row.Search,
                CreatedTime = c.row.CreatedTime,

            });
            //4. Sort
            query2 = query2.OrderByDynamic<OrderAccountViewModel>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<OrderAccountViewModel>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
        public async Task AddRange(List<OrderAccount> listOrderAccount)
        {
            if(db != null)
            {
                await db.OrderAccounts.AddRangeAsync(listOrderAccount);
                await db.SaveChangesAsync();
            }
        }
        public async Task<OrderAccount> CheckExit(int accountBuyId, int orderId)
        {
            if(db != null)
            {
                var query =  await (from row in db.OrderAccounts
                                   where row.Active == 1 && row.OrderId == orderId && row.AccountBuyId == accountBuyId
                                   select row).FirstOrDefaultAsync();
                return query;
                
            }
            return null;
        }
        public async Task<AccountBuyViewModel> DetailAccountBuyViewModel(int accountBuyId, int orderId)
        {
            if (db != null)
            {
                var query = await (from row in db.OrderAccounts
                                   join ab in db.AccountBuys on row.AccountBuyId equals ab.Id
                                   join oap in db.OrderAccountPaymentStatuses on row.OrderPaymentStatusId equals oap.Id
                                   join oas in db.OrderAccountStatuses on row.OrderAccountStatusId equals oas.Id
                                   where row.Active == 1 && ab.Active == 1 && oap.Active == 1 && oas.Active == 1
                                   && row.OrderId == orderId && row.AccountBuyId == accountBuyId
                                   select new AccountBuyViewModel
                                   {
                                       Id = ab.Id,
                                       FullName = ab.FullName,
                                       Username = ab.Username,
                                       Password = ab.Password,
                                       Description = row.Description,
                                       OrderPaymentStatusName = oap.Name,
                                       OrderPaymentStatusId = row.OrderPaymentStatusId,
                                       OrderAccountStatusId = row.OrderAccountStatusId,
                                       OrderAccountStatusName = oas.Name,
                                       CreatedTime = ab.CreatedTime,
                                       OrderAccountId = row.Id
                                   }).FirstOrDefaultAsync();
                return query;
            }
            return null;
        }
    }
}


