
using QLBH_Dion.Models;
using QLBH_Dion.Repository;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;
using QLBH_Dion.Constants;

namespace QLBH_Dion.Services
{
    public class AccountBuyService : IAccountBuyService
    {
        IAccountBuyRepository accountBuyRepository;
        public AccountBuyService(
            IAccountBuyRepository _accountBuyRepository
            )
        {
            accountBuyRepository = _accountBuyRepository;
        }
        public async Task Add(AccountBuy obj)
        {
            obj.Active = 1;
            obj.CreatedTime = DateTime.Now;
            await accountBuyRepository.Add(obj);
        }

        public int Count()
        {
            var result = accountBuyRepository.Count();
            return result;
        }

        public async Task Delete(AccountBuy obj)
        {
            obj.Active = 0;
            await accountBuyRepository.Delete(obj);
        }

        public async Task<int> DeletePermanently(int? id)
        {
            return await accountBuyRepository.DeletePermanently(id);
        }

        public async Task<List<AccountBuy>> Detail(int? id)
        {
            return await accountBuyRepository.Detail(id);
        }
        public async Task<AccountBuyViewModel> Detail2(int accountBuyId)
        {
            return await accountBuyRepository.Detail2(accountBuyId);
        }

        public async Task<List<AccountBuy>> List()
        {
            return await accountBuyRepository.List();
        }

        public async Task<List<AccountBuy>> ListPaging(int pageIndex, int pageSize)
        {
            return await accountBuyRepository.ListPaging(pageIndex, pageSize);
        }

        public async Task<DTResult<AccountBuy>> ListServerSide(AccountBuyDTParameters parameters)
        {
            return await accountBuyRepository.ListServerSide(parameters);
        }

        public async Task<List<AccountBuy>> Search(string keyword)
        {
            return await accountBuyRepository.Search(keyword);
        }

        public async Task Update(AccountBuy obj)
        {
            await accountBuyRepository.Update(obj);
        }
        public async Task<object> ListAccountBuyByOrderId(PagingModel obj)
        {
            return await accountBuyRepository.ListAccountBuyByOrderId(obj);
        }
        public async Task LockAccountBuy(AccountBuy accountBuy)
        {
            accountBuy.AccountStatusId = AccountStatusId.LOCKED;
            await accountBuyRepository.LockAccountBuy(accountBuy);
        }
        public async Task<bool> CheckExits(string username)
        {
            return await accountBuyRepository.CheckExits(username);
        }
    }
}

