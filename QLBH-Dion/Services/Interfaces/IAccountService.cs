
using QLBH_Dion.Models;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Services.Interfaces
{
    public interface IAccountService : IBaseService<Account>
    {
        Task<DTResult<AccountViewModel>> ListServerSide(AccountDTParameters parameters);
        Task<LoginViewModel> Login(LoginViewModel model);
        Task<AccountViewModel> GetInfoAccountById(int accountId);
        Task<bool> UpdateFullNameById(int accountId, string fullName);
        Task<bool> UpdatePasswordById(int accountId, ChangePassword changePassword);
        Task<AccountViewModel> Detail2(int accountId);
        Task<bool> IsUserNameExist(string userName, int accountId);
        Task<bool> UpdateByViewModel(UpdateAccountViewModel updateAccountViewModel);
        Task LockAccount(Account account);
        Task<List<DeviceToken>> GetDeviceToken();
        Task<bool> CheckExits(string username);
    }
}
