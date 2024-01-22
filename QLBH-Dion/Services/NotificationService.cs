
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
            public class NotificationService : INotificationService
            {
                INotificationRepository notificationRepository;
                public NotificationService(
                    INotificationRepository _notificationRepository
                    )
                {
                    notificationRepository = _notificationRepository;
                }
                public async Task Add(Notification obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await notificationRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = notificationRepository.Count();
                    return result;
                }
        
                public async Task Delete(Notification obj)
                {
                    obj.Active = 0;
                    await notificationRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await notificationRepository.DeletePermanently(id);
                }
        
                public async Task<List<Notification>> Detail(int? id)
                {
                    return await notificationRepository.Detail(id);
                }
        
                public async Task<List<Notification>> List()
                {
                    return await notificationRepository.List();
                }
        
                public async Task<List<Notification>> ListPaging(int pageIndex, int pageSize)
                {
                    return await notificationRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<NotificationViewModel>> ListServerSide(NotificationDTParameters parameters)
                {
                    return await notificationRepository.ListServerSide(parameters);
                }
        
                public async Task<List<Notification>> Search(string keyword)
                {
                    return await notificationRepository.Search(keyword);
                }
        
                public async Task Update(Notification obj)
                {
                    await notificationRepository.Update(obj);
                }
            }
        }
    
    