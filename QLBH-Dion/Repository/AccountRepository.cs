
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

namespace QLBH_Dion.Repository
{
    public class AccountRepository : IAccountRepository
    {
        QLBHContext db;
        public AccountRepository(QLBHContext _db)
        {
            db = _db;
        }


        public async Task<List<Account>> List()
        {
            if (db != null)
            {
                return await (
                    from row in db.Accounts
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }

            return null;
        }


        public async Task<List<Account>> Search(string keyword)
        {
            if (db != null)
            {
                return await (
                    from row in db.Accounts
                    where (row.Active == 1 && (row.FullName.Contains(keyword) || row.Username.Contains(keyword)))
                    orderby row.Id descending
                    select row
                ).ToListAsync();
            }
            return null;
        }


        public async Task<List<Account>> ListPaging(int pageIndex, int pageSize)
        {
            int offSet = 0;
            offSet = (pageIndex - 1) * pageSize;
            if (db != null)
            {
                return await (
                    from row in db.Accounts
                    where (row.Active == 1)
                    orderby row.Id descending
                    select row
                ).Skip(offSet).Take(pageSize).ToListAsync();
            }
            return null;
        }


        public async Task<List<Account>> Detail(int? id)
        {
            if (db != null)
            {
                return await (
                    from row in db.Accounts
                    where (row.Active == 1 && row.Id == id)
                    select row)
                    .ToListAsync();
            }

            return null;
        }
        public async Task<AccountViewModel> Detail2(int accountId)
        {
            if(db != null)
            {
                var accountDetail = await (from a in db.Accounts
                              from ast in db.AccountStatuses
                              from r in db.Roles
                              where a.Active == 1 && ast.Active == 1 && r.Active == 1 &&
                              a.Id == accountId && r.Id == a.RoleId && ast.Id == a.AccountStatusId
                              select new AccountViewModel
                              {
                                  Id = a.Id,
                                  GuidId = a.GuidId,
                                  Username = a.Username,
                                  FullName = a.FullName,
                                  RoleId = a.RoleId,
                                  RoleName = r.Name,
                                  AccountStatusId = a.AccountStatusId,
                                  AccountStatusName = ast.Name
                              }).FirstOrDefaultAsync();
                return accountDetail;
            }
            return null;
        }
        public async Task<bool> SetDevice(Account account)
        {
            if(db != null)
            {
                db.Accounts.Attach(account);
                db.Entry(account).Property(x => x.GuidId).IsModified = true;
                db.Entry(account).Property(x => x.GuidIdApp).IsModified = true;
                return await db.SaveChangesAsync() == 1;
            }
            return false;
        }
        public async Task<Account> Add(Account obj)
        {
            if (db != null)
            {
                try
                {
                    await db.Accounts.AddAsync(obj);
                    await db.SaveChangesAsync();
                    return obj;
                }
                catch (Exception e)
                {

                    throw;
                }

            }
            return null;
        }
        public async Task<bool> CheckExits(string username)
        {
            if(db != null)
            {
                try
                {
                    return await db.Accounts.AnyAsync(x => x.Username.Equals(username) && x.Active == 1);
                }
                catch
                {
                    throw;
                }
            }
            return false;
        }

        public async Task Update(Account obj)
        {
            if (db != null)
            {
                //Update that object
                db.Accounts.Attach(obj);
                db.Entry(obj).Property(x => x.AccountStatusId).IsModified = true;
                db.Entry(obj).Property(x => x.Password).IsModified = true;
                db.Entry(obj).Property(x => x.FullName).IsModified = true;
                db.Entry(obj).Property(x => x.RoleId).IsModified = true;
                //Commit the transaction
                await db.SaveChangesAsync();
            }
        }


        public async Task Delete(Account obj)
        {
            if (db != null)
            {
                //Update that obj
                db.Accounts.Attach(obj);
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
                var obj = await db.Accounts.FirstOrDefaultAsync(x => x.Id == objId);

                if (obj != null)
                {
                    //Delete that obj
                    db.Accounts.Remove(obj);

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
                    from row in db.Accounts
                    where row.Active == 1
                    select row
                            ).Count();
            }

            return result;
        }
        public async Task<DTResult<AccountViewModel>> ListServerSide(AccountDTParameters parameters)
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
            var query = from row in db.Accounts
                        join ast in db.AccountStatuses on row.AccountStatusId equals ast.Id
                        join r in db.Roles on row.RoleId equals r.Id
                        where row.Active == 1 && ast.Active == 1 && r.Active == 1
                        select new AccountViewModel
                        {
                            Id = row.Id,
                            FullName = row.FullName,
                            Username = row.Username,
                            RoleName = r.Name,
                            RoleId = r.Id,
                            AccountStatusId = row.AccountStatusId,
                            AccountStatusName = ast.Name,
                            CreatedTime = row.CreatedTime
                        };

            recordTotal = await query.CountAsync();
            //2. Fillter
            if (!String.IsNullOrEmpty(searchAll))
            {
                searchAll = searchAll.ToLower();
                query = query.Where(c => EF.Functions.Collate(c.RoleName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                                         EF.Functions.Collate(c.AccountStatusName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                                         EF.Functions.Collate(c.Username.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                                         EF.Functions.Collate(c.FullName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General)) ||
                                         EF.Functions.Collate(c.CreatedTime.ToCustomString().ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(searchAll, SQLParams.Latin_General))
                );
            }
            foreach (var item in parameters.Columns)
            {
                var fillter = item.Search.Value.Trim().ToLower();
                if (fillter.Length > 0)
                {
                    switch (item.Data)
                    {
                        case "id":
                            query = query.Where(c => c.Id.ToString().Trim().Contains(fillter));
                            break;
                        case "username":
                            query = query.Where(c => EF.Functions.Collate(c.Username.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter, SQLParams.Latin_General)));
                            break;
                        case "roleName":
                            query = query.Where(c => (c.RoleId.ToString() ?? "").Contains(fillter));
                            break;
                        case "accountStatusName":
                            query = query.Where(c => (c.AccountStatusId.ToString() ?? "").Contains(fillter));
                            break;
                        case "fullName":
                            query = query.Where(c => EF.Functions.Collate(c.FullName.ToLower(), SQLParams.Latin_General).Contains(EF.Functions.Collate(fillter, SQLParams.Latin_General)));
                            break;
                        case "createdTime":
                            var searchDateArrs = fillter.Split(',');

                            if (searchDateArrs.Length == 2)
                            {
                                //không có ngày b?t ??u
                                if (string.IsNullOrEmpty(searchDateArrs[0]))
                                {
                                    query = query.Where(c => c.CreatedTime <= Convert.ToDateTime(searchDateArrs[1]));
                                }
                                //không có ngày k?t thúc
                                else if (string.IsNullOrEmpty(searchDateArrs[1]))
                                {
                                    query = query.Where(c => c.CreatedTime >= Convert.ToDateTime(searchDateArrs[0]));
                                }
                                //có c? 2
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
            var query2 = query.Select(c => new AccountViewModel()
            {
                Id = c.Id,
                AccountStatusId = c.AccountStatusId,
                AccountStatusName = c.AccountStatusName,
                Username = c.Username,
                FullName = c.FullName,                
                CreatedTime = c.CreatedTime,             
                RoleId = c.RoleId,
                RoleName = c.RoleName,

            });
            //4. Sort
            query2 = query2.OrderByDynamic<AccountViewModel>(orderCritirea, orderDirectionASC ? LinqExtensions.Order.Asc : LinqExtensions.Order.Desc);
            recordFiltered = await query2.CountAsync();
            //5. Return data
            return new DTResult<AccountViewModel>()
            {
                data = await query2.Skip(parameters.Start).Take(parameters.Length).ToListAsync(),
                draw = parameters.Draw,
                recordsFiltered = recordFiltered,
                recordsTotal = recordTotal
            };
        }
        public async Task<LoginViewModel> Login(LoginViewModel model)
        {
            var account = await (from acc in db.Accounts
                                 from role in db.Roles
                                 where acc.Active == 1 && role.Active == 1 && acc.RoleId == role.Id /*&& acc.AccountStatusId == AccountStatusId.ACTIVE*/
                                 && acc.Username == model.Username
                                 select new LoginViewModel
                                 {
                                     Username = acc.Username,
                                     FullName = acc.FullName,
                                     RoleName = role.Name,
                                     Password = acc.Password,
                                     Id = acc.Id,
                                     RoleId = acc.RoleId,
                                     AccountStatusId = acc.AccountStatusId
                                 })
                          .FirstOrDefaultAsync();
            return account;
        }
        public async Task<AccountViewModel> GetInfoAccountById(int accountId)
        {
            var account = await (from a in db.Accounts
                                 from r in db.Roles
                                 where a.Active == 1 && r.Active == 1 && a.Id == accountId && a.RoleId == r.Id
                                 select new AccountViewModel
                                 {
                                     FullName = a.FullName,
                                     RoleName = r.Name,
                                     Username = a.Username
                                 }).FirstOrDefaultAsync();
            return account;
        }
        public async Task<bool> UpdateFullNameById(int accountId, string fullName)
        {
            if (db != null)
            {
                var searchAccount = await db.Accounts.FirstAsync(c => c.Id == accountId);
                searchAccount.FullName = fullName;
                db.Attach(searchAccount);
                db.Entry(searchAccount).Property(s => s.FullName).IsModified = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdatePasswordById(int accountId, ChangePassword changePassword)
        {
            if (db != null)
            {
                var searchAccount = await db.Accounts.FirstAsync(c => c.Id == accountId);
                if (changePassword.OldPassword.ToHash256() == searchAccount.Password)
                {
                    searchAccount.Password = changePassword.NewPassword.ToHash256();
                    db.Attach(searchAccount);
                    db.Entry(searchAccount).Property(s => s.Password).IsModified = true;
                    await db.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task<bool> IsUserNameExist(string userName, int accountId)
        {
            return await db.Accounts.AnyAsync(s => s.Active == 1 && s.Username.Equals(userName) && s.Id != accountId);
        }
        public async Task LockAccount(Account account)
        {
            if(db != null)
            {
                db.Accounts.Attach(account);
                db.Entry(account).Property(x => x.AccountStatusId).IsModified = true;
                await db.SaveChangesAsync();
            }
        }
        public async Task<List<DeviceToken>> GetDeviceToken()
        {
            // lấy tất cả device token của trưởng phòng mua hàng, giám đốc, admin
            var query = await (from row in db.Accounts
                               where row.Active == 1 &&
                               (row.RoleId == RoleId.PURCHASING_MANAGER || row.RoleId == RoleId.ADMIN || row.RoleId == RoleId.DIRECTOR)
                               select new DeviceToken
                               {
                                   GuidId = row.GuidId,
                                   GuidIdApp = row.GuidIdApp,
                               }
                             ).ToListAsync();
            return query;
        }
    }
}


