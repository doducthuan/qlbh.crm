
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
    public interface INotificationRepository
    {
        Task<List<Notification>> List();

        Task<List<Notification>> Search(string keyword);

        Task<List<Notification>> ListPaging(int pageIndex, int pageSize);

        Task<List<Notification>> Detail(int? postId);

        Task<Notification> Add(Notification Notification);

        Task Update(Notification Notification);

        Task Delete(Notification Notification);

        Task<int> DeletePermanently(int? NotificationId);

        int Count();

        Task<DTResult<NotificationViewModel>> ListServerSide(NotificationDTParameters parameters);
    }
}
