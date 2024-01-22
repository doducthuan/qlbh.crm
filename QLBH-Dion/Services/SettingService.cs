
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
            public class SettingService : ISettingService
            {
                ISettingRepository settingRepository;
                public SettingService(
                    ISettingRepository _settingRepository
                    )
                {
                    settingRepository = _settingRepository;
                }
                public async Task Add(Setting obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await settingRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = settingRepository.Count();
                    return result;
                }
        
                public async Task Delete(Setting obj)
                {
                    obj.Active = 0;
                    await settingRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await settingRepository.DeletePermanently(id);
                }
        
                public async Task<List<Setting>> Detail(int? id)
                {
                    return await settingRepository.Detail(id);
                }
        
                public async Task<List<Setting>> List()
                {
                    return await settingRepository.List();
                }
        
                public async Task<List<Setting>> ListPaging(int pageIndex, int pageSize)
                {
                    return await settingRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<Setting>> ListServerSide(SettingDTParameters parameters)
                {
                    return await settingRepository.ListServerSide(parameters);
                }
        
                public async Task<List<Setting>> Search(string keyword)
                {
                    return await settingRepository.Search(keyword);
                }
        
                public async Task Update(Setting obj)
                {
                    await settingRepository.Update(obj);
                }
            }
        }
    
    