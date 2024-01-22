
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
            public class ActivityLogService : IActivityLogService
            {
                IActivityLogRepository activityLogRepository;
                public ActivityLogService(
                    IActivityLogRepository _activityLogRepository
                    )
                {
                    activityLogRepository = _activityLogRepository;
                }
                public async Task Add(ActivityLog obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await activityLogRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = activityLogRepository.Count();
                    return result;
                }
        
                public async Task Delete(ActivityLog obj)
                {
                    obj.Active = 0;
                    await activityLogRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await activityLogRepository.DeletePermanently(id);
                }
        
                public async Task<List<ActivityLog>> Detail(int? id)
                {
                    return await activityLogRepository.Detail(id);
                }
        
                public async Task<List<ActivityLog>> List()
                {
                    return await activityLogRepository.List();
                }
        
                public async Task<List<ActivityLog>> ListPaging(int pageIndex, int pageSize)
                {
                    return await activityLogRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<ActivityLogViewModel>> ListServerSide(ActivityLogDTParameters parameters)
                {
                    return await activityLogRepository.ListServerSide(parameters);
                }
        
                public async Task<List<ActivityLog>> Search(string keyword)
                {
                    return await activityLogRepository.Search(keyword);
                }
        
                public async Task Update(ActivityLog obj)
                {
                    await activityLogRepository.Update(obj);
                }
            }
        }
    
    