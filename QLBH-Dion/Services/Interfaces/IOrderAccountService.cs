
using QLBH_Dion.Models;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Services.Interfaces
{
    public interface IOrderAccountService : IBaseService<OrderAccount>
    {
        Task<DTResult<OrderAccountViewModel>> ListServerSide(OrderAccountDTParameters parameters);
        Task<bool> AddRange(OrderAccountAddRange obj);
        Task<bool> Delete2(int accountBuyId, int orderId);
        Task<AccountBuyViewModel> DetailAccountBuyViewModel(int accountBuyId, int orderId);
        Task UpdateByViewModel(OrderAccount obj);
    }
}
