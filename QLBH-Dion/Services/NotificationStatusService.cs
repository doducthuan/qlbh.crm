
        using QLBH_Dion.Models;
        using QLBH_Dion.Repository;
        using QLBH_Dion.Services.Interfaces;
        using QLBH_Dion.Util;
        using QLBH_Dion.Util.Parameters;
        using QLBH_Dion.Models.ViewModels;
        using System;
        using System.Collections.Generic;
        using System.Threading.Tasks;
        
        namespace QLBH_Dion.Services
        {
            public class NotificationStatusService : INotificationStatusService
            {
                INotificationStatusRepository notificationStatusRepository;
                public NotificationStatusService(
                    INotificationStatusRepository _notificationStatusRepository
                    )
                {
                    notificationStatusRepository = _notificationStatusRepository;
                }
                public async Task Add(NotificationStatus obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await notificationStatusRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = notificationStatusRepository.Count();
                    return result;
                }
        
                public async Task Delete(NotificationStatus obj)
                {
                    obj.Active = 0;
                    await notificationStatusRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await notificationStatusRepository.DeletePermanently(id);
                }
        
                public async Task<List<NotificationStatus>> Detail(int? id)
                {
                    return await notificationStatusRepository.Detail(id);
                }
        
                public async Task<List<NotificationStatus>> List()
                {
                    return await notificationStatusRepository.List();
                }
        
                public async Task<List<NotificationStatus>> ListPaging(int pageIndex, int pageSize)
                {
                    return await notificationStatusRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<NotificationStatus>> ListServerSide(NotificationStatusDTParameters parameters)
                {
                    return await notificationStatusRepository.ListServerSide(parameters);
                }
        
                public async Task<List<NotificationStatus>> Search(string keyword)
                {
                    return await notificationStatusRepository.Search(keyword);
                }
        
                public async Task Update(NotificationStatus obj)
                {
                    await notificationStatusRepository.Update(obj);
                }
            }
        }
    
    