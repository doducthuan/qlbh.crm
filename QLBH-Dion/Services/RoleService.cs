
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
            public class RoleService : IRoleService
            {
                IRoleRepository roleRepository;
                public RoleService(
                    IRoleRepository _roleRepository
                    )
                {
                    roleRepository = _roleRepository;
                }
                public async Task Add(Role obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await roleRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = roleRepository.Count();
                    return result;
                }
        
                public async Task Delete(Role obj)
                {
                    obj.Active = 0;
                    await roleRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await roleRepository.DeletePermanently(id);
                }
        
                public async Task<List<Role>> Detail(int? id)
                {
                    return await roleRepository.Detail(id);
                }
        
                public async Task<List<Role>> List()
                {
                    return await roleRepository.List();
                }
        
                public async Task<List<Role>> ListPaging(int pageIndex, int pageSize)
                {
                    return await roleRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<Role>> ListServerSide(RoleDTParameters parameters)
                {
                    return await roleRepository.ListServerSide(parameters);
                }
        
                public async Task<List<Role>> Search(string keyword)
                {
                    return await roleRepository.Search(keyword);
                }
        
                public async Task Update(Role obj)
                {
                    await roleRepository.Update(obj);
                }
            }
        }
    
    