
using QLBH_Dion.Models;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Services.Interfaces
{
    public interface IAccountBuyService : IBaseService<AccountBuy>
    {
        Task<DTResult<AccountBuy>> ListServerSide(AccountBuyDTParameters parameters);
        Task<object> ListAccountBuyByOrderId(PagingModel obj);
        Task<AccountBuyViewModel> Detail2(int accountBuyId);
        Task LockAccountBuy(AccountBuy accountBuy);
        Task<bool> CheckExits(string username);
    }
}
