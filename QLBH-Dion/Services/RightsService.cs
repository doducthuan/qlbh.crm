
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
            public class RightsService : IRightsService
            {
                IRightsRepository rightsRepository;
                public RightsService(
                    IRightsRepository _rightsRepository
                    )
                {
                    rightsRepository = _rightsRepository;
                }
                public async Task Add(Right obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await rightsRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = rightsRepository.Count();
                    return result;
                }
        
                public async Task Delete(Right obj)
                {
                    obj.Active = 0;
                    await rightsRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await rightsRepository.DeletePermanently(id);
                }
        
                public async Task<List<Right>> Detail(int? id)
                {
                    return await rightsRepository.Detail(id);
                }
        
                public async Task<List<Right>> List()
                {
                    return await rightsRepository.List();
                }
        
                public async Task<List<Right>> ListPaging(int pageIndex, int pageSize)
                {
                    return await rightsRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<Right>> ListServerSide(RightsDTParameters parameters)
                {
                    return await rightsRepository.ListServerSide(parameters);
                }
        
                public async Task<List<Right>> Search(string keyword)
                {
                    return await rightsRepository.Search(keyword);
                }
        
                public async Task Update(Right obj)
                {
                    await rightsRepository.Update(obj);
                }
            }
        }
    
    