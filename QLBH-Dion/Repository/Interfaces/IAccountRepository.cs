
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
    public interface IAccountRepository
    {
        Task<List<Account>> List();

        Task<List<Account>> Search(string keyword);

        Task<List<Account>> ListPaging(int pageIndex, int pageSize);

        Task<List<Account>> Detail(int? postId);
        Task<AccountViewModel> Detail2(int accountId);

        Task<Account> Add(Account Account);

        Task Update(Account Account);

        Task Delete(Account Account);

        Task<int> DeletePermanently(int? AccountId);

        int Count();

        Task<DTResult<AccountViewModel>> ListServerSide(AccountDTParameters parameters);
        Task<LoginViewModel> Login(LoginViewModel model);
        Task<AccountViewModel> GetInfoAccountById(int accountId);
        Task<bool> UpdateFullNameById(int accountId, string fullName);
        Task<bool> UpdatePasswordById(int accountId, ChangePassword changePassword);
        Task<bool> IsUserNameExist(string userName, int accountId);
        Task LockAccount(Account account);
        Task<bool> SetDevice(Account account);
        Task<List<DeviceToken>> GetDeviceToken();
        Task<bool> CheckExits(string username);
    }
}
