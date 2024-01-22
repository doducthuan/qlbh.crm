
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
            public class OrderAccountPaymentStatusService : IOrderAccountPaymentStatusService
            {
                IOrderAccountPaymentStatusRepository orderAccountPaymentStatusRepository;
                public OrderAccountPaymentStatusService(
                    IOrderAccountPaymentStatusRepository _orderAccountPaymentStatusRepository
                    )
                {
                    orderAccountPaymentStatusRepository = _orderAccountPaymentStatusRepository;
                }
                public async Task Add(OrderAccountPaymentStatus obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await orderAccountPaymentStatusRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = orderAccountPaymentStatusRepository.Count();
                    return result;
                }
        
                public async Task Delete(OrderAccountPaymentStatus obj)
                {
                    obj.Active = 0;
                    await orderAccountPaymentStatusRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await orderAccountPaymentStatusRepository.DeletePermanently(id);
                }
        
                public async Task<List<OrderAccountPaymentStatus>> Detail(int? id)
                {
                    return await orderAccountPaymentStatusRepository.Detail(id);
                }
        
                public async Task<List<OrderAccountPaymentStatus>> List()
                {
                    return await orderAccountPaymentStatusRepository.List();
                }
        
                public async Task<List<OrderAccountPaymentStatus>> ListPaging(int pageIndex, int pageSize)
                {
                    return await orderAccountPaymentStatusRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<OrderAccountPaymentStatus>> ListServerSide(OrderAccountPaymentStatusDTParameters parameters)
                {
                    return await orderAccountPaymentStatusRepository.ListServerSide(parameters);
                }
        
                public async Task<List<OrderAccountPaymentStatus>> Search(string keyword)
                {
                    return await orderAccountPaymentStatusRepository.Search(keyword);
                }
        
                public async Task Update(OrderAccountPaymentStatus obj)
                {
                    await orderAccountPaymentStatusRepository.Update(obj);
                }
            }
        }
    
    