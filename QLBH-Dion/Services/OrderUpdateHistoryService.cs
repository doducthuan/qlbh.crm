
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
    public class OrderUpdateHistoryService : IOrderUpdateHistoryService
    {
        IOrderUpdateHistoryRepository orderUpdateHistoryRepository;
        public OrderUpdateHistoryService(
            IOrderUpdateHistoryRepository _orderUpdateHistoryRepository
            )
        {
            orderUpdateHistoryRepository = _orderUpdateHistoryRepository;
        }
        public async Task Add(OrderUpdateHistory obj)
        {
            obj.Active = 1;
            obj.CreatedTime = DateTime.Now;
            await orderUpdateHistoryRepository.Add(obj);
        }

        public int Count()
        {
            var result = orderUpdateHistoryRepository.Count();
            return result;
        }

        public async Task Delete(OrderUpdateHistory obj)
        {
            obj.Active = 0;
            await orderUpdateHistoryRepository.Delete(obj);
        }

        public async Task<int> DeletePermanently(int? id)
        {
            return await orderUpdateHistoryRepository.DeletePermanently(id);
        }

        public async Task<List<OrderUpdateHistory>> Detail(int? id)
        {
            return await orderUpdateHistoryRepository.Detail(id);
        }

        public async Task<List<OrderUpdateHistory>> List()
        {
            return await orderUpdateHistoryRepository.List();
        }

        public async Task<List<OrderUpdateHistory>> ListPaging(int pageIndex, int pageSize)
        {
            return await orderUpdateHistoryRepository.ListPaging(pageIndex, pageSize);
        }

        //public async Task<DTResult<OrderUpdateHistoryViewModel>> ListServerSide(OrderUpdateHistoryDTParameters parameters)
        //{
        //    return await orderUpdateHistoryRepository.ListServerSide(parameters);
        //}

        public async Task<List<OrderUpdateHistory>> Search(string keyword)
        {
            return await orderUpdateHistoryRepository.Search(keyword);
        }

        public async Task Update(OrderUpdateHistory obj)
        {
            await orderUpdateHistoryRepository.Update(obj);
        }
        public async Task<List<OrderUpdateHistory>> GetChangeOrderNew(int orderId)
        {
            return await orderUpdateHistoryRepository.GetChangeOrderNew(orderId);
        }
    }
}

