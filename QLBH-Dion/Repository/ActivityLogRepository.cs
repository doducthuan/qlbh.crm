
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
            public class ActivityLogRepository: IActivityLogRepository
                {
                    QLBHContext db;
                    public ActivityLogRepository(QLBHContext _db)
                    {
                        db = _db;
                    }


            public async Task <List<ActivityLog>> List()
            {
                            if(db != null)
                    {
                        return await(
                            from row in db.ActivityLogs
                        where(row.Active == 1)
                        orderby row.Id descending
                        select row
                        ).ToListAsync();
                    }

                    return null;
                }


            public async Task <List< ActivityLog>> Search(string keyword)
            {
                if(db != null)
                {
                    return await(
                        from row in db.ActivityLogs
                                    where(row.Active == 1 && (row.Name.Contains(keyword) || row.Description.Contains(keyword)))
                                    orderby row.Id descending
                                    select row
                    ).ToListAsync();
                }
                return null;
            }


            public async Task <List<ActivityLog>> ListPaging(int pageIndex, int pageSize)
            {
                int offSet = 0;
                offSet = (pageIndex - 1) * pageSize;
                if (db != null) {
                    return await(
                        from row in db.ActivityLogs
                                    where(row.Active == 1)
                                    orderby row.Id descending
                                    select row
                    ).Skip(offSet).Take(pageSize).ToListAsync();
                }
                return null;
            }


            public async Task <List< ActivityLog>> Detail(int ? id)
            {
                if (db != null) {
                    return await(
                        from row in db.ActivityLogs
                                    where(row.Active == 1 && row.Id == id)
                                    select row)
                        .ToListAsync();
                }

                return null;
            }


            public async Task <ActivityLog> Add(ActivityLog obj)
            {
                if (db != null) {
                    await db.ActivityLogs.AddAsync(obj);
                    await db.SaveChangesAsync();
                    return obj;
                }
                return null;
            }


            public async Task Update(ActivityLog obj)
            {
                if (db != null) {
                    //Update that object
                    db.ActivityLogs.Attach(obj);
                    db.Entry(obj).Property(x => x.Active).IsModified = true;
db.Entry(obj).Property(x => x.AccountId).IsModified = true;
db.Entry(obj).Property(x => x.Name).IsModified = true;
db.Entry(obj).Property(x => x.EntityCode).IsModified = true;
db.Entry(obj).Property(x => x.Info).IsModified = true;
db.Entry(obj).Property(x => x.ObjectOld).IsModified = true;
db.Entry(obj).Property(x => x.ObjectNew).IsModified = true;
db.Entry(obj).Property(x => x.Url).IsModified = true;
db.Entry(obj).Property(x => x.UrlSource).IsModified = true;
db.Entry(obj).Property(x => x.IpAddress).IsModified = true;
db.Entry(obj).Property(x => x.Device).IsModified = true;
db.Entry(obj).Property(x => x.Browser).IsModified = true;
db.Entry(obj).Property(x => x.Os).IsModified = true;
db.Entry(obj).Property(x => x.UserAgent).IsModified = true;
db.Entry(obj).Property(x => x.Description).IsModified = true;

                    //Commit the transaction
                    await db.SaveChangesAsync();
                }
            }


            public async Task Delete(ActivityLog obj)
            {
                if (db != null) {
                    //Update that obj
                    db.ActivityLogs.Attach(obj);
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
                    var obj = await db.ActivityLogs.FirstOrDefaultAsync(x => x.Id == objId);

                    if (obj != null) {
                        //Delete that obj
                        db.ActivityLogs.Remove(obj);

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
                        from row in db.ActivityLogs
                                    where row.Active == 1
                                    select row
                                ).Count();
                }

                return result;
            }
            public async Task <DTResult<ActivityLogViewModel>> ListServerSide(ActivityLogDTParameters parameters)
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
                var query = from row in db.ActivityLogs 

                                    join a in db.Accounts on row.AccountId equals a.Id 

                    where row.Active == 1
                                    && a.Active == 1

                    select new {
                        row,a
                    };
                
                recordTotal = await query.CountAsync();
                //2. Fillter
                if (!String.IsNullOrEmpty(searchAll)) {
                    searchAll = searchAll.ToLower();
                    query = query.Where(c =>
                        EF.Functions.Collate(c.row.Id.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Active.ToString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Name.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.EntityCode.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Info.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.ObjectOld.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.ObjectNew.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Url.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.UrlSource.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.IpAddress.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Device.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Browser.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.Os.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
EF.Functions.Collate(c.row.UserAgent.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
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
 case "entityCode":
                query = query.Where(c => (c.row.EntityCode ?? "").Contains(fillter));
                break;
 case "info":
                query = query.Where(c => (c.row.Info ?? "").Contains(fillter));
                break;
 case "objectOld":
                query = query.Where(c => (c.row.ObjectOld ?? "").Contains(fillter));
                break;
 case "objectNew":
                query = query.Where(c => (c.row.ObjectNew ?? "").Contains(fillter));
                break;
 case "url":
                query = query.Where(c => (c.row.Url ?? "").Contains(fillter));
                break;
 case "urlSource":
                query = query.Where(c => (c.row.UrlSource ?? "").Contains(fillter));
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
                
                                if (parameters.AccountIds.Count > 0)
                                {
                                    query = query.Where(c => parameters.AccountIds.Contains(c.row.Account.Id));
                                }
                                

                //3.Query second
                var query2 = query.Select(c => new ActivityLogViewModel()
                {
                    Id = c.row.Id,
Active = c.row.Active,
AccountId = c.a.Id,
Name = c.row.Name,
EntityCode = c.row.EntityCode,
Info = c.row.Info,
ObjectOld = c.row.ObjectOld,
ObjectNew = c.row.ObjectNew,
Url = c.row.Url,
UrlSource = c.row.UrlSource,
IpAddress = c.row.IpAddress,
Device = c.row.Device,
Browser = c.row.Browser,
Os = c.row.Os,
UserAgent = c.row.UserAgent,
Description = c.row.Description,
CreatedTime = c.row.CreatedTime,

                });
                //4. Sort
                query2 = query2.OrderByDynamic<ActivityLogViewModel>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
                recordFiltered = await query2.CountAsync();
                //5. Return data
                return new DTResult<ActivityLogViewModel>()
                {
                    data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                        draw = parameters.Draw,
                        recordsFiltered = recordFiltered,
                        recordsTotal = recordTotal
                };
            }
        }
    }


