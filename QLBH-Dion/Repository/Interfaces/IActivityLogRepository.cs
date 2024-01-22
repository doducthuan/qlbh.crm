
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
    public interface IActivityLogRepository
    {
        Task<List<ActivityLog>> List();

        Task<List<ActivityLog>> Search(string keyword);

        Task<List<ActivityLog>> ListPaging(int pageIndex, int pageSize);

        Task<List<ActivityLog>> Detail(int? postId);

        Task<ActivityLog> Add(ActivityLog ActivityLog);

        Task Update(ActivityLog ActivityLog);

        Task Delete(ActivityLog ActivityLog);

        Task<int> DeletePermanently(int? ActivityLogId);

        int Count();

        Task<DTResult<ActivityLogViewModel>> ListServerSide(ActivityLogDTParameters parameters);
    }
}
