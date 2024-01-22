
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
            public class OrderStatusService : IOrderStatusService
            {
                IOrderStatusRepository orderStatusRepository;
                public OrderStatusService(
                    IOrderStatusRepository _orderStatusRepository
                    )
                {
                    orderStatusRepository = _orderStatusRepository;
                }
                public async Task Add(OrderStatus obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await orderStatusRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = orderStatusRepository.Count();
                    return result;
                }
        
                public async Task Delete(OrderStatus obj)
                {
                    obj.Active = 0;
                    await orderStatusRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await orderStatusRepository.DeletePermanently(id);
                }
        
                public async Task<List<OrderStatus>> Detail(int? id)
                {
                    return await orderStatusRepository.Detail(id);
                }
        
                public async Task<List<OrderStatus>> List()
                {
                    return await orderStatusRepository.List();
                }
        
                public async Task<List<OrderStatus>> ListPaging(int pageIndex, int pageSize)
                {
                    return await orderStatusRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<OrderStatus>> ListServerSide(OrderStatusDTParameters parameters)
                {
                    return await orderStatusRepository.ListServerSide(parameters);
                }
        
                public async Task<List<OrderStatus>> Search(string keyword)
                {
                    return await orderStatusRepository.Search(keyword);
                }
        
                public async Task Update(OrderStatus obj)
                {
                    await orderStatusRepository.Update(obj);
                }
            }
        }
    
    