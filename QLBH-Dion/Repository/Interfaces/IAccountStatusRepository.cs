
using QLBH_Dion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace QLBH_Dion.Repository
{
    public interface IAccountStatusRepository
    {
        Task<List<AccountStatus>> List();

        Task<List<AccountStatus>> Search(string keyword);

        Task<List<AccountStatus>> ListPaging(int pageIndex, int pageSize);

        Task<List<AccountStatus>> Detail(int? postId);

        Task<AccountStatus> Add(AccountStatus AccountStatus);

        Task Update(AccountStatus AccountStatus);

        Task Delete(AccountStatus AccountStatus);

        Task<int> DeletePermanently(int? AccountStatusId);

        int Count();

        Task<DTResult<AccountStatus>> ListServerSide(AccountStatusDTParameters parameters);
    }
}
