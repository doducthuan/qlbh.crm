
using QLBH_Dion.Models;
using QLBH_Dion.Repository;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using QLBH_Dion.Constants;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Services
{
    public class AccountService : IAccountService
    {
        IAccountRepository accountRepository;
        IHttpContextAccessor httpContextAccessor;
        ITokenService tokenService;
        public AccountService(
            IAccountRepository _accountRepository,
            IHttpContextAccessor _httpContextAccessor,
            ITokenService _tokenService
            )
        {
            accountRepository = _accountRepository;
            httpContextAccessor = _httpContextAccessor;
            tokenService = _tokenService;
        }
        public async Task Add(Account obj)
        {
            obj.Active = 1;
            obj.CreatedTime = DateTime.Now;
            obj.Password = obj.Password.ToHash256();
            await accountRepository.Add(obj);
        }

        public int Count()
        {
            var result = accountRepository.Count();
            return result;
        }

        public async Task Delete(Account obj)
        {
            obj.Active = 0;
            await accountRepository.Delete(obj);
        }

        public async Task<int> DeletePermanently(int? id)
        {
            return await accountRepository.DeletePermanently(id);
        }

        public async Task<List<Account>> Detail(int? id)
        {
            return await accountRepository.Detail(id);
        }
        public async Task<AccountViewModel> Detail2(int accountId)
        {
            return await accountRepository.Detail2(accountId);
        }
        public async Task<List<Account>> List()
        {
            return await accountRepository.List();
        }

        public async Task<List<Account>> ListPaging(int pageIndex, int pageSize)
        {
            return await accountRepository.ListPaging(pageIndex, pageSize);
        }

        public async Task<DTResult<AccountViewModel>> ListServerSide(AccountDTParameters parameters)
        {
            return await accountRepository.ListServerSide(parameters);
        }

        public async Task<List<Account>> Search(string keyword)
        {
            return await accountRepository.Search(keyword);
        }

        public async Task Update(Account obj)
        {
            await accountRepository.Update(obj);
        }
        public async Task<LoginViewModel> Login(LoginViewModel model)
        {
            var account = await accountRepository.Login(model);
            if (account == null)
            {
                return null;
            }
            if (account.Password != model.Password.ToHash256())
            {
                return null;
            }
            if (account.AccountStatusId == AccountStatusId.ACTIVE)
            {
                account.Token = tokenService.GenerateToken(account);
                httpContextAccessor.HttpContext.Session.SetInt32("UserId", account.Id);
                httpContextAccessor.HttpContext.Session.SetInt32("RoleId", account.RoleId);
                account.AccountStatusId = 0;
            }
            account.Id = 0;
            //account.RoleId = 0;
            account.Password = "";
            return account;
        }
        public async Task<AccountViewModel> GetInfoAccountById(int accountId)
        {
            return await accountRepository.GetInfoAccountById(accountId);
        }
        public async Task<bool> UpdateFullNameById(int accountId, string fullName)
        {
            return await accountRepository.UpdateFullNameById(accountId, fullName);
        }
        public async Task<bool> UpdatePasswordById(int accountId, ChangePassword changePassword)
        {
            return await accountRepository.UpdatePasswordById(accountId, changePassword);
        }
        public async Task<bool> IsUserNameExist(string userName, int accountId)
        {
            return await accountRepository.IsUserNameExist(userName, accountId);
        }
        public async Task<bool> UpdateByViewModel(UpdateAccountViewModel updateAccountViewModel)
        {
            var updateAccounts = await accountRepository.Detail(updateAccountViewModel.Id);
            if (updateAccounts == null)
            {
                return false;
            }

            updateAccounts[0].FullName = updateAccountViewModel.FullName;
            updateAccounts[0].RoleId = updateAccountViewModel.RoleId;
            updateAccounts[0].AccountStatusId = updateAccountViewModel.AccountStatusId;
            if (!string.IsNullOrEmpty(updateAccountViewModel.Password))
            {
                updateAccounts[0].Password = updateAccountViewModel.Password.ToHash256();
            }
            await accountRepository.Update(updateAccounts[0]);
            return true;
        }
        public async Task LockAccount(Account account)
        {
            account.AccountStatusId = AccountStatusId.LOCKED;
            await accountRepository.LockAccount(account);
        }
        public async Task<List<DeviceToken>> GetDeviceToken()
        {
            return await accountRepository.GetDeviceToken();
        }
        public async Task<bool> CheckExits(string username)
        {
            return await accountRepository.CheckExits(username);
        }
    }
}

