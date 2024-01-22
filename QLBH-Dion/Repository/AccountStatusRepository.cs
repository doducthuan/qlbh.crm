
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

    namespace QLBH_Dion.Repository
        {
            public class AccountStatusRepository: IAccountStatusRepository
                {
                    QLBHContext db;
                    public AccountStatusRepository(QLBHContext _db)
                    {
                        db = _db;
                    }


            public async Task <List<AccountStatus>> List()
            {
                            if(db != null)
                    {
                        return await(
                            from row in db.AccountStatuses
                        where(row.Active == 1)
                        orderby row.Id descending
                        select row
                        ).ToListAsync();
                    }

                    return null;
                }


            public async Task <List< AccountStatus>> Search(string keyword)
            {
                if(db != null)
                {
                    return await(
                        from row in db.AccountStatuses
                                    where(row.Active == 1 && (row.Name.Contains(keyword) || row.Description.Contains(keyword)))
                                    orderby row.Id descending
                                    select row
                    ).ToListAsync();
                }
                return null;
            }


            public async Task <List<AccountStatus>> ListPaging(int pageIndex, int pageSize)
            {
                int offSet = 0;
                offSet = (pageIndex - 1) * pageSize;
                if (db != null) {
                    return await(
                        from row in db.AccountStatuses
                                    where(row.Active == 1)
                                    orderby row.Id descending
                                    select row
                    ).Skip(offSet).Take(pageSize).ToListAsync();
                }
                return null;
            }


            public async Task <List< AccountStatus>> Detail(int ? id)
            {
                if (db != null) {
                    return await(
                        from row in db.AccountStatuses
                                    where(row.Active == 1 && row.Id == id)
                                    select row)
                        .ToListAsync();
                }

                return null;
            }


            public async Task <AccountStatus> Add(AccountStatus obj)
            {
                if (db != null) {
                    await db.AccountStatuses.AddAsync(obj);
                    await db.SaveChangesAsync();
                    return obj;
                }
                return null;
            }


            public async Task Update(AccountStatus obj)
            {
                if (db != null) {
                    //Update that object
                    db.AccountStatuses.Attach(obj);
                    db.Entry(obj).Property(x => x.Active).IsModified = true;
db.Entry(obj).Property(x => x.Name).IsModified = true;
db.Entry(obj).Property(x => x.Description).IsModified = true;

                    //Commit the transaction
                    await db.SaveChangesAsync();
                }
            }


            public async Task Delete(AccountStatus obj)
            {
                if (db != null) {
                    //Update that obj
                    db.AccountStatuses.Attach(obj);
                    db.Entry(obj).Property(x => x.Active).IsModified = true;

                    //Commit the transaction
                    await db.SaveChangesAsync();
                }
            }

            public async Task<int> DeletePermanently(int ? objId)
            {
                            int result = 0;

                if (db != null) {
                    //Find the obj for specific obj id
                    var obj = await db.AccountStatuses.FirstOrDefaultAsync(x => x.Id == objId);

                    if (obj != null) {
                        //Delete that obj
                        db.AccountStatuses.Remove(obj);

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

                if (db != null) {
                    //Find the obj for specific obj id
                    result = (
                        from row in db.AccountStatuses
                                    where row.Active == 1
                                    select row
                                ).Count();
                }

                return result;
            }
            public async Task <DTResult<AccountStatus>> ListServerSide(AccountStatusDTParameters parameters)
            {
                //0. Options
                string searchAll = parameters.SearchAll.Trim();//Trim text
                string orderCritirea = "Id";//Set default critirea
                int recordTotal, recordFiltered;
                bool orderDirectionASC = true;//Set default ascending
                if (parameters.Order != null) {
                    orderCritirea = parameters.Columns[parameters.Order[0].Column].Data;
                    orderDirectionASC = parameters.Order[0].Dir == DTOrderDir.ASC;
                }
                //1. Join
                var query = from row in db.AccountStatuses 

                                    
                    where row.Active == 1
                                    
                    select new {
                        row
                    };
                
                recordTotal = await query.CountAsync();
                //2. Fillter
                if (!String.IsNullOrEmpty(searchAll)) {
                    searchAll = searchAll.ToLower();
                    query = query.Where(c =>
                        EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Active.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Description.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))

                    );
                }
                foreach(var item in parameters.Columns)
                {
                    var fillter = item.Search.Value.Trim();
                    if (fillter.Length > 0) {
                        switch (item.Data) {
                            case "id":
                    query = query.Where(c => c.row.Id.ToString().Trim().Contains(fillter));
                    break;
case "active":
                    query = query.Where(c => c.row.Active.ToString().Trim().Contains(fillter));
                    break;
 case "name":
                query = query.Where(c => (c.row.Name ?? "").Contains(fillter));
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

                        }
                    }
                }
                
                //3.Query second
                var query2 = query.Select(c => new AccountStatus()
                {
                    Id = c.row.Id,
Active = c.row.Active,
Name = c.row.Name,
Description = c.row.Description,
CreatedTime = c.row.CreatedTime,

                });
                //4. Sort
                query2 = query2.OrderByDynamic<AccountStatus>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
                recordFiltered = await query2.CountAsync();
                //5. Return data
                return new DTResult<AccountStatus>()
                {
                    data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                        draw = parameters.Draw,
                        recordsFiltered = recordFiltered,
                        recordsTotal = recordTotal
                };
            }
        }
    }


