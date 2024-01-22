
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
    public interface ISettingRepository
    {
        Task<List<Setting>> List();

        Task<List<Setting>> Search(string keyword);

        Task<List<Setting>> ListPaging(int pageIndex, int pageSize);

        Task<List<Setting>> Detail(int? postId);

        Task<Setting> Add(Setting Setting);

        Task Update(Setting Setting);

        Task Delete(Setting Setting);

        Task<int> DeletePermanently(int? SettingId);

        int Count();

        Task<DTResult<Setting>> ListServerSide(SettingDTParameters parameters);
    }
}
