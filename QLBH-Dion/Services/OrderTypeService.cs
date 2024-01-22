
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
            public class OrderTypeService : IOrderTypeService
            {
                IOrderTypeRepository orderTypeRepository;
                public OrderTypeService(
                    IOrderTypeRepository _orderTypeRepository
                    )
                {
                    orderTypeRepository = _orderTypeRepository;
                }
                public async Task Add(OrderType obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await orderTypeRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = orderTypeRepository.Count();
                    return result;
                }
        
                public async Task Delete(OrderType obj)
                {
                    obj.Active = 0;
                    await orderTypeRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await orderTypeRepository.DeletePermanently(id);
                }
        
                public async Task<List<OrderType>> Detail(int? id)
                {
                    return await orderTypeRepository.Detail(id);
                }
        
                public async Task<List<OrderType>> List()
                {
                    return await orderTypeRepository.List();
                }
        
                public async Task<List<OrderType>> ListPaging(int pageIndex, int pageSize)
                {
                    return await orderTypeRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<OrderType>> ListServerSide(OrderTypeDTParameters parameters)
                {
                    return await orderTypeRepository.ListServerSide(parameters);
                }
        
                public async Task<List<OrderType>> Search(string keyword)
                {
                    return await orderTypeRepository.Search(keyword);
                }
        
                public async Task Update(OrderType obj)
                {
                    await orderTypeRepository.Update(obj);
                }
            }
        }
    
    