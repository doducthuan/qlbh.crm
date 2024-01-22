
using QLBH_Dion.Models;
using QLBH_Dion.Repository;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;
using QLBH_Dion.Constants;

namespace QLBH_Dion.Services
{
    public class OrderAccountService : IOrderAccountService
    {
        IOrderAccountRepository orderAccountRepository;
        public OrderAccountService(
            IOrderAccountRepository _orderAccountRepository
            )
        {
            orderAccountRepository = _orderAccountRepository;
        }
        public async Task Add(OrderAccount obj)
        {
            obj.Active = 1;
            obj.CreatedTime = DateTime.Now;
            await orderAccountRepository.Add(obj);
        }

        public int Count()
        {
            var result = orderAccountRepository.Count();
            return result;
        }

        public async Task Delete(OrderAccount obj)
        {
            obj.Active = 0;
            await orderAccountRepository.Delete(obj);
        }

        public async Task<int> DeletePermanently(int? id)
        {
            return await orderAccountRepository.DeletePermanently(id);
        }

        public async Task<List<OrderAccount>> Detail(int? id)
        {
            return await orderAccountRepository.Detail(id);
        }

        public async Task<List<OrderAccount>> List()
        {
            return await orderAccountRepository.List();
        }

        public async Task<List<OrderAccount>> ListPaging(int pageIndex, int pageSize)
        {
            return await orderAccountRepository.ListPaging(pageIndex, pageSize);
        }

        public async Task<DTResult<OrderAccountViewModel>> ListServerSide(OrderAccountDTParameters parameters)
        {
            return await orderAccountRepository.ListServerSide(parameters);
        }

        public async Task<List<OrderAccount>> Search(string keyword)
        {
            return await orderAccountRepository.Search(keyword);
        }

        public async Task Update(OrderAccount obj)
        {
            await orderAccountRepository.Update(obj);
        }
        public async Task<bool> AddRange(OrderAccountAddRange obj)
        {
            List<OrderAccount> listOrderAccount = new List<OrderAccount>();
            foreach (int accountBuyId in obj.ListOrderAccountId)
            {
                OrderAccount orderAccount = new OrderAccount();
                orderAccount.AccountBuyId = accountBuyId;
                orderAccount.OrderId = obj.OrderId;
                orderAccount.OrderPaymentStatusId = OrderPaymentStatusId.PAID;
                orderAccount.OrderAccountStatusId = OrderAccountStatusId.WAITING;
                orderAccount.CreatedTime = DateTime.Now;
                if (await orderAccountRepository.CheckExit(orderAccount.AccountBuyId, orderAccount.OrderId) == null)
                {
                    orderAccount.Active = 1;
                    listOrderAccount.Add(orderAccount);
                }

            }
            if (listOrderAccount.Count == obj.ListOrderAccountId.Count)
            {
                await orderAccountRepository.AddRange(listOrderAccount);
                return true;
            }
            return false;
        }
        public async Task<bool> Delete2(int accountBuyId, int orderId)
        {
            var orderAccount = await orderAccountRepository.CheckExit(accountBuyId, orderId);
            if (orderAccount != null)
            {
                orderAccount.Active = 0;
                await orderAccountRepository.Delete(orderAccount);
                return true;
            }
            return false;
        }
        public async Task<AccountBuyViewModel> DetailAccountBuyViewModel(int accountBuyId, int orderId)
        {
            return await orderAccountRepository.DetailAccountBuyViewModel(accountBuyId, orderId);
        }
        public async Task UpdateByViewModel(OrderAccount obj)
        {
            await orderAccountRepository.UpdateByViewModel(obj);
        }
    }
}

