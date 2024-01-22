
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
            public class OrderAccountStatusService : IOrderAccountStatusService
            {
                IOrderAccountStatusRepository orderAccountStatusRepository;
                public OrderAccountStatusService(
                    IOrderAccountStatusRepository _orderAccountStatusRepository
                    )
                {
                    orderAccountStatusRepository = _orderAccountStatusRepository;
                }
                public async Task Add(OrderAccountStatus obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await orderAccountStatusRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = orderAccountStatusRepository.Count();
                    return result;
                }
        
                public async Task Delete(OrderAccountStatus obj)
                {
                    obj.Active = 0;
                    await orderAccountStatusRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await orderAccountStatusRepository.DeletePermanently(id);
                }
        
                public async Task<List<OrderAccountStatus>> Detail(int? id)
                {
                    return await orderAccountStatusRepository.Detail(id);
                }
        
                public async Task<List<OrderAccountStatus>> List()
                {
                    return await orderAccountStatusRepository.List();
                }
        
                public async Task<List<OrderAccountStatus>> ListPaging(int pageIndex, int pageSize)
                {
                    return await orderAccountStatusRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<OrderAccountStatus>> ListServerSide(OrderAccountStatusDTParameters parameters)
                {
                    return await orderAccountStatusRepository.ListServerSide(parameters);
                }
        
                public async Task<List<OrderAccountStatus>> Search(string keyword)
                {
                    return await orderAccountStatusRepository.Search(keyword);
                }
        
                public async Task Update(OrderAccountStatus obj)
                {
                    await orderAccountStatusRepository.Update(obj);
                }
            }
        }
    
    