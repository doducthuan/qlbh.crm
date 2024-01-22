
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
            public class RoleRightsService : IRoleRightsService
            {
                IRoleRightRepository roleRightsRepository;
                public RoleRightsService(
                    IRoleRightRepository _roleRightsRepository
                    )
                {
                    roleRightsRepository = _roleRightsRepository;
                }
                public async Task Add(RoleRight obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await roleRightsRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = roleRightsRepository.Count();
                    return result;
                }
        
                public async Task Delete(RoleRight obj)
                {
                    obj.Active = 0;
                    await roleRightsRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await roleRightsRepository.DeletePermanently(id);
                }
        
                public async Task<List<RoleRight>> Detail(int? id)
                {
                    return await roleRightsRepository.Detail(id);
                }
        
                public async Task<List<RoleRight>> List()
                {
                    return await roleRightsRepository.List();
                }
        
                public async Task<List<RoleRight>> ListPaging(int pageIndex, int pageSize)
                {
                    return await roleRightsRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<RoleRightsViewModel>> ListServerSide(RoleRightsDTParameters parameters)
                {
                    return await roleRightsRepository.ListServerSide(parameters);
                }
        
                public async Task<List<RoleRight>> Search(string keyword)
                {
                    return await roleRightsRepository.Search(keyword);
                }
        
                public async Task Update(RoleRight obj)
                {
                    await roleRightsRepository.Update(obj);
                }
            }
        }
    
    