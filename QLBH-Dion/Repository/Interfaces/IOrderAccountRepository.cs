
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
    public interface IOrderAccountRepository
    {
        Task<List<OrderAccount>> List();

        Task<List<OrderAccount>> Search(string keyword);

        Task<List<OrderAccount>> ListPaging(int pageIndex, int pageSize);

        Task<List<OrderAccount>> Detail(int? postId);

        Task<OrderAccount> Add(OrderAccount OrderAccount);

        Task Update(OrderAccount OrderAccount);
        Task UpdateByViewModel(OrderAccount obj);

        Task Delete(OrderAccount OrderAccount);

        Task<int> DeletePermanently(int? OrderAccountId);

        int Count();

        Task<DTResult<OrderAccountViewModel>> ListServerSide(OrderAccountDTParameters parameters);
        Task AddRange(List<OrderAccount> orderAccounts);
        Task<OrderAccount> CheckExit(int accountBuyId, int orderId);
        Task<AccountBuyViewModel> DetailAccountBuyViewModel(int accountBuyId, int orderId);
    }
}
