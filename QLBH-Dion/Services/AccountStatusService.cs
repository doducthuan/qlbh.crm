
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
            public class AccountStatusService : IAccountStatusService
            {
                IAccountStatusRepository accountStatusRepository;
                public AccountStatusService(
                    IAccountStatusRepository _accountStatusRepository
                    )
                {
                    accountStatusRepository = _accountStatusRepository;
                }
                public async Task Add(AccountStatus obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await accountStatusRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = accountStatusRepository.Count();
                    return result;
                }
        
                public async Task Delete(AccountStatus obj)
                {
                    obj.Active = 0;
                    await accountStatusRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await accountStatusRepository.DeletePermanently(id);
                }
        
                public async Task<List<AccountStatus>> Detail(int? id)
                {
                    return await accountStatusRepository.Detail(id);
                }
        
                public async Task<List<AccountStatus>> List()
                {
                    return await accountStatusRepository.List();
                }
        
                public async Task<List<AccountStatus>> ListPaging(int pageIndex, int pageSize)
                {
                    return await accountStatusRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<AccountStatus>> ListServerSide(AccountStatusDTParameters parameters)
                {
                    return await accountStatusRepository.ListServerSide(parameters);
                }
        
                public async Task<List<AccountStatus>> Search(string keyword)
                {
                    return await accountStatusRepository.Search(keyword);
                }
        
                public async Task Update(AccountStatus obj)
                {
                    await accountStatusRepository.Update(obj);
                }
            }
        }
    
    