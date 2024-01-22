
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

namespace QLBH_Dion.Repository
{
    public class OrderUpdateHistoryRepository : IOrderUpdateHistoryRepository
    {
        QLBHContext db;
        public OrderUpdateHistoryRepository(QLBHContext _db)
        {
            db = _db;
        }


        public async Task<List<OrderUpdateHistory>> List()
        {
            if (db != null)
            {
                return await (
                    from row in db.OrderUpdateHistories
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }

            return null;
        }


        public async Task<List<OrderUpdateHistory>> Search(string keyword)
        {
            if (db != null)
            {
                return await (
                    from row in db.OrderUpdateHistories
                    where (row.Active == 1 && (row.Name.Contains(keyword) || row.Description.Contains(keyword)))
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }
            return null;
        }


        public async Task<List<OrderUpdateHistory>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await (
                    from row in db.OrderUpdateHistories
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }


        public async Task<List<OrderUpdateHistory>> Detail(int? id)
        {
            if (db != null)
            {
                return await (
                    from row in db.OrderUpdateHistories
                    where (row.Active == 1 && row.Id == id)
                    select row)
                    .ToListAsync();
            }

            return null;
        }


        public async Task<OrderUpdateHistory> Add(OrderUpdateHistory obj)
        {
            if (db != null)
            {
                await db.OrderUpdateHistories.AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            return null;
        }


        public async Task Update(OrderUpdateHistory obj)
        {
            if (db != null)
            {
                //Update that object
                db.OrderUpdateHistories.Attach(obj);
                db.Entry(obj).Property(x => x.Active).IsModified = true;
                db.Entry(obj).Property(x => x.OrderId).IsModified = true;
                db.Entry(obj).Property(x => x.AccountId).IsModified = true;
                db.Entry(obj).Property(x => x.Name).IsModified = true;
                db.Entry(obj).Property(x => x.ObjectOld).IsModified = true;
                db.Entry(obj).Property(x => x.ObjectNew).IsModified = true;
                db.Entry(obj).Property(x => x.Change).IsModified = true;
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                db.Entry(obj).Property(x => x.IpAddress).IsModified = true;
                db.Entry(obj).Property(x => x.Device).IsModified = true;
                db.Entry(obj).Property(x => x.Browser).IsModified = true;
                db.Entry(obj).Property(x => x.Os).IsModified = true;
                db.Entry(obj).Property(x => x.UserAgent).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }


        public async Task Delete(OrderUpdateHistory obj)
        {
            if (db != null)
            {
                //Update that obj
                db.OrderUpdateHistories.Attach(obj);
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
                var obj = await db.OrderUpdateHistories.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.OrderUpdateHistories.Remove(obj);

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
                    from row in db.OrderUpdateHistories
                    where row.Active == 1
                    select row
                            ).Count();
            }

            return result;
        }
        public async Task<DTResult<OrderUpdateHistory>> ListServerSide(OrderUpdateHistoryDTParameters parameters)
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
            var query = from row in db.OrderUpdateHistories


                        where row.Active == 1

                        select new
                        {
                            row
                        };

            recordTotal = await query.CountAsync();
            //2. Fillter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c =>
                    EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Active.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.OrderId.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.AccountId.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.ObjectOld.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.ObjectNew.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Change.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Description.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.IpAddress.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Device.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Browser.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.Os.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.row.UserAgent.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))
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
                        case "active":
                            query = query.Where(c => c.row.Active.ToString().Trim().Contains(fillter));
                            break;
                        case "orderId":
                            query = query.Where(c => c.row.OrderId.ToString().Trim().Contains(fillter));
                            break;
                        case "accountId":
                            query = query.Where(c => c.row.AccountId.ToString().Trim().Contains(fillter));
                            break;
                        case "name":
                            query = query.Where(c => (c.row.Name ?? "").Contains(fillter));
                            break;
                        case "objectOld":
                            query = query.Where(c => (c.row.ObjectOld ?? "").Contains(fillter));
                            break;
                        case "objectNew":
                            query = query.Where(c => (c.row.ObjectNew ?? "").Contains(fillter));
                            break;
                        case "change":
                            query = query.Where(c => (c.row.Change ?? "").Contains(fillter));
                            break;
                        case "description":
                            query = query.Where(c => (c.row.Description ?? "").Contains(fillter));
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
                        case "ipAddress":
                            query = query.Where(c => (c.row.IpAddress ?? "").Contains(fillter));
                            break;
                        case "device":
                            query = query.Where(c => (c.row.Device ?? "").Contains(fillter));
                            break;
                        case "browser":
                            query = query.Where(c => (c.row.Browser ?? "").Contains(fillter));
                            break;
                        case "os":
                            query = query.Where(c => (c.row.Os ?? "").Contains(fillter));
                            break;
                        case "userAgent":
                            query = query.Where(c => (c.row.UserAgent ?? "").Contains(fillter));
                            break;

                    }
                }
            }

            //3.Query second
            var query2 = query.Select(c => new OrderUpdateHistory()
            {
                Id = c.row.Id,
                Active = c.row.Active,
                OrderId = c.row.OrderId,
                AccountId = c.row.AccountId,
                Name = c.row.Name,
                ObjectOld = c.row.ObjectOld,
                ObjectNew = c.row.ObjectNew,
                Change = c.row.Change,
                Description = c.row.Description,
                CreatedTime = c.row.CreatedTime,
                IpAddress = c.row.IpAddress,
                Device = c.row.Device,
                Browser = c.row.Browser,
                Os = c.row.Os,
                UserAgent = c.row.UserAgent,

            });
            //4. Sort
            query2 = query2.OrderByDynamic<OrderUpdateHistory>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<OrderUpdateHistory>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
        public async Task<List<OrderUpdateHistory>> GetChangeOrderNew(int orderId)
        {
            if(db != null)
            {
                var orderUpdateHistory = await(from row in db.OrderUpdateHistories
                                         where row.OrderId == orderId && row.Active == 1
                                         orderby row.Id descending
                                         select row).ToListAsync();
                return orderUpdateHistory;
            }
            return null;
        }
        public DatabaseFacade GetDatabase()
        {
            return db.Database;
        }
    }
}


