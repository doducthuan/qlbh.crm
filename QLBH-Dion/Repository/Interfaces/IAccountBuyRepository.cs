
using QLBH_Dion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Repository
{
    public interface IAccountBuyRepository
    {
        Task<List<AccountBuy>> List();

        Task<List<AccountBuy>> Search(string keyword);

        Task<List<AccountBuy>> ListPaging(int pageIndex, int pageSize);

        Task<List<AccountBuy>> Detail(int? postId);
        Task<AccountBuyViewModel> Detail2(int accountBuyId);

        Task<AccountBuy> Add(AccountBuy AccountBuy);

        Task Update(AccountBuy AccountBuy);

        Task Delete(AccountBuy AccountBuy);

        Task<int> DeletePermanently(int? AccountBuyId);

        int Count();

        Task<DTResult<AccountBuy>> ListServerSide(AccountBuyDTParameters parameters);
        Task<object> ListAccountBuyByOrderId(PagingModel obj);
        Task LockAccountBuy(AccountBuy accountBuy);
        Task<bool> CheckExits(string username);
    }
}
