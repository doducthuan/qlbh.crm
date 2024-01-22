
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
    public class AccountBuyRepository : IAccountBuyRepository
    {
        QLBHContext db;
        public AccountBuyRepository(QLBHContext _db)
        {
            db = _db;
        }


        public async Task<List<AccountBuy>> List()
        {
            if (db != null)
            {
                return await (
                    from row in db.AccountBuys
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }

            return null;
        }


        public async Task<List<AccountBuy>> Search(string keyword)
        {
            if (db != null)
            {
                return await (
                    from row in db.AccountBuys
                    where (row.Active == 1 && (row.FullName.Contains(keyword) || row.Description.Contains(keyword)))
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }
            return null;
        }


        public async Task<List<AccountBuy>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await (
                    from row in db.AccountBuys
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }


        public async Task<List<AccountBuy>> Detail(int? id)
        {
            if (db != null)
            {
                return await (
                    from row in db.AccountBuys
                    where (row.Active == 1 && row.Id == id)
                    select row)
                    .ToListAsync();
            }

            return null;
        }


        public async Task<AccountBuy> Add(AccountBuy obj)
        {
            if (db != null)
            {
                await db.AccountBuys.AddAsync(obj);
                await db.SaveChangesAsync();
                return obj;
            }
            return null;
        }


        public async Task Update(AccountBuy obj)
        {
            if (db != null)
            {
                //Update that object
                db.AccountBuys.Attach(obj);
                db.Entry(obj).Property(x => x.AccountStatusId).IsModified = true;
                db.Entry(obj).Property(x => x.Username).IsModified = true;
                db.Entry(obj).Property(x => x.Password).IsModified = true;
                db.Entry(obj).Property(x => x.FullName).IsModified = true;
                db.Entry(obj).Property(x => x.Email).IsModified = true;
                db.Entry(obj).Property(x => x.Description).IsModified = true;
                db.Entry(obj).Property(x => x.Info).IsModified = true;
                //db.Entry(obj).Property(x => x.Active).IsModified = true;
                db.Entry(obj).Property(x => x.Search).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }


        public async Task Delete(AccountBuy obj)
        {
            if (db != null)
            {
                //Update that obj
                db.AccountBuys.Attach(obj);
                db.Entry(obj).Property(x => x.Active).IsModified = true;

                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }
        public async Task<AccountBuyViewModel> Detail2(int accountBuyId)
        {
            if(db != null)
            {
                var query = await(from row in db.AccountBuys
                            join ast in db.AccountStatuses on row.AccountStatusId equals ast.Id
                            where row.Active == 1 && ast.Active == 1 && row.Id == accountBuyId
                            select new AccountBuyViewModel
                            {
                                Id = row.Id,
                                AccountStatusId = row.AccountStatusId,
                                AccountStatusName = ast.Name,
                                Username = row.Username,
                                Password = row.Password,
                                FullName = row.FullName,
                                Email = row.Email,
                                CreatedTime = row.CreatedTime,
                            }).FirstOrDefaultAsync();
                return query;
            }
            return null;
        }

        public async Task<int> DeletePermanently(int? objId)
        {
            int result = 0;

            if (db != null)
            {
                //Find the obj for specific obj id
                var obj = await db.AccountBuys.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.AccountBuys.Remove(obj);

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
                    from row in db.AccountBuys
                    where row.Active == 1
                    select row
                            ).Count();
            }

            return result;
        }
        public async Task<DTResult<AccountBuy>> ListServerSide(AccountBuyDTParameters parameters)
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
            var query = from row in db.AccountBuys
                        join ast in db.AccountStatuses on row.AccountStatusId equals ast.Id

                        where row.Active == 1 && ast.Active == 1

                        select new AccountBuyViewModel
                        {
                            Id = row.Id,
                            AccountStatusId = row.AccountStatusId,
                            AccountStatusName = ast.Name,
                            Username = row.Username,
                            Password = row.Password,
                            FullName = row.FullName,
                            Email = row.Email,
                            CreatedTime = row.CreatedTime,
                        };

            recordTotal = await query.CountAsync();
            //2. Fillter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c =>
                    EF.Functions.Collate(c.Username.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.Password.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.FullName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.Email.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                    EF.Functions.Collate(c.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))
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
                            query = query.Where(c => c.Id.ToString().Trim().Contains(fillter));
                            break;
                        case "accountStatusName":
                            query = query.Where(c => c.AccountStatusId.ToString().Trim().Contains(fillter));
                            break;
                        case "username":
                            query = query.Where(c => EF.Functions.Collate(c.Username.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter, SQLParams.Latin_General)));
                            break;
                        case "password":
                            query = query.Where(c => EF.Functions.Collate(c.Password.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter, SQLParams.Latin_General)));
                            break;
                        case "fullName":
                            query = query.Where(c => EF.Functions.Collate(c.FullName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter, SQLParams.Latin_General)));
                            break;
                        case "email":
                            query = query.Where(c => EF.Functions.Collate(c.Email.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter, SQLParams.Latin_General)));
                            break;
                        case "createdTime":
                            var searchDateArrs = fillter.Split(',');

                            if (searchDateArrs.Length == 2)
                            {
                                //không có ngày bắt đầu
                                if (string.IsNullOrEmpty(searchDateArrs[0]))
                                {
                                    query = query.Where(c => c.CreatedTime <= Convert.ToDateTime(searchDateArrs[1]));
                                }
                                //không có ngày kết thúc
                                else if (string.IsNullOrEmpty(searchDateArrs[1]))
                                {
                                    query = query.Where(c => c.CreatedTime >= Convert.ToDateTime(searchDateArrs[0]));
                                }
                                //có cả 2
                                else
                                {
                                    query = query.Where(c => c.CreatedTime >= Convert.ToDateTime(searchDateArrs[0]) && c.CreatedTime <= Convert.ToDateTime(searchDateArrs[1]));
                                }

                            }
                            break;
                        default:
                            break;

                    }
                }
            }

            //3.Query second
            var query2 = query.Select(c => new AccountBuyViewModel()
            {
                Id = c.Id,
                AccountStatusId = c.AccountStatusId,
                AccountStatusName = c.AccountStatusName,
                Username = c.Username,
                Password = c.Password,
                FullName = c.FullName,
                Email = c.Email,
                CreatedTime = c.CreatedTime
            });
            //4. Sort
            query2 = query2.OrderByDynamic<AccountBuyViewModel>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<AccountBuy>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
        public async Task<object> ListAccountBuyByOrderId(PagingModel obj)
        {
            var query = from row in db.OrderAccounts
                        join ab in db.AccountBuys on row.AccountBuyId equals ab.Id
                        join oap in db.OrderAccountPaymentStatuses on row.OrderPaymentStatusId equals oap.Id
                        join oas in db.OrderAccountStatuses on row.OrderAccountStatusId equals oas.Id
                        where row.Active == 1 && row.OrderId == obj.Id && ab.Active == 1 && oap.Active == 1 && oas.Active == 1
                        orderby row.Id descending
                        select new AccountBuyViewModel
                        {
                            Id = ab.Id,
                            FullName = ab.FullName,
                            Username = ab.Username,
                            Password = ab.Password,
                            Description = row.Description,
                            OrderPaymentStatusName = oap.Name,
                            OrderAccountStatusId = row.OrderAccountStatusId,
                            OrderPaymentStatusId = row.OrderPaymentStatusId,
                            OrderAccountStatusName = oas.Name,
                            CreatedTime = ab.CreatedTime,
                            OrderAccountId = row.Id
                        };
            var total = await query.CountAsync();
            if (!string.IsNullOrEmpty(obj.SearchAll))
            {
                var searchAll = obj.SearchAll.Trim().ToLower();
                query = query.Where(x => x.FullName.ToLower().Contains(searchAll) ||
                                         x.Username.ToLower().Contains(searchAll) ||
                                         x.Password.ToLower().Contains(searchAll) ||
                                         x.Description.ToLower().Contains(searchAll) ||
                                         x.CreatedTime.ToCustomString().Contains(searchAll));
            }
            var totalFilter = await query.CountAsync();
            var firstRowOnPage = total > 0 ? (totalFilter > 0 ? (obj.PageIndex - 1) * obj.PageSize + 1 : 0) : 0;
            var lastRowOnPage = Math.Min(obj.PageIndex * obj.PageSize, totalFilter);
            return new
            {
                Data = await query.Skip(((obj.PageIndex - 1) * obj.PageSize)).Take(obj.PageSize).ToListAsync(),
                Total = total,
                TotalFilter = totalFilter,
                TotalPage = totalFilter % obj.PageSize > 0 ? totalFilter / obj.PageSize + 1 : totalFilter / obj.PageSize,
                FirstRowOnPage = firstRowOnPage,
                LastRowOnPage = lastRowOnPage,
                PageInfo = $"Đang xem {firstRowOnPage} đến {lastRowOnPage} trong tổng số {totalFilter} mục " + (total > totalFilter ? $"(được lọc từ {total} mục)" : ""),
                IndexStart = (obj.PageIndex - 1) * obj.PageSize + 1,
            };
        }
        public async Task LockAccountBuy(AccountBuy accountBuy)
        {
            if(db != null)
            {
                db.AccountBuys.Attach(accountBuy);
                db.Entry(accountBuy).Property(x => x.AccountStatusId).IsModified = true;
                await db.SaveChangesAsync();
            }
        }
        public async Task<bool> CheckExits(string username)
        {
            if (db != null)
            {
                try
                {
                    return await db.AccountBuys.AnyAsync(x => x.Username.Equals(username) && x.Active == 1);
                }
                catch
                {
                    throw;
                }
            }
            return false;
        }
    }
}


