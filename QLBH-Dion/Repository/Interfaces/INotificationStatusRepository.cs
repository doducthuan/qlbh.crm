
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
    public interface INotificationStatusRepository
    {
        Task<List<NotificationStatus>> List();

        Task<List<NotificationStatus>> Search(string keyword);

        Task<List<NotificationStatus>> ListPaging(int pageIndex, int pageSize);

        Task<List<NotificationStatus>> Detail(int? postId);

        Task<NotificationStatus> Add(NotificationStatus NotificationStatus);

        Task Update(NotificationStatus NotificationStatus);

        Task Delete(NotificationStatus NotificationStatus);

        Task<int> DeletePermanently(int? NotificationStatusId);

        int Count();

        Task<DTResult<NotificationStatus>> ListServerSide(NotificationStatusDTParameters parameters);
    }
}
