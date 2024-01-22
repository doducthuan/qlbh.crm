
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
    public class OrdersService : IOrdersService
    {
        IOrderRepository ordersRepository;
        IAccountRepository accountRepository;
        IOrderUpdateHistoryRepository orderUpdateHistoryRepository;
        public OrdersService(
            IOrderRepository _ordersRepository,
            IAccountRepository _accountRepository
,

            IOrderUpdateHistoryRepository _orderUpdateHistoryRepository
            )
        {
            ordersRepository = _ordersRepository;
            accountRepository = _accountRepository;
            orderUpdateHistoryRepository = _orderUpdateHistoryRepository;
        }
        public async Task Add(Order obj)
        {
            obj.Active = 1;
            obj.CreatedTime = DateTime.Now;
            await ordersRepository.Add(obj);
        }

        public int Count()
        {
            var result = ordersRepository.Count();
            return result;
        }

        public async Task Delete(Order obj)
        {
            obj.Active = 0;
            await ordersRepository.Delete(obj);
        }

        public async Task<int> DeletePermanently(int? id)
        {
            return await ordersRepository.DeletePermanently(id);
        }

        public async Task<List<Order>> Detail(int? id)
        {
            return await ordersRepository.Detail(id);
        }
        public async Task<OrdersViewModel> Detail2(int orderId)
        {
            return await ordersRepository.Detail2(orderId);
        }
        public async Task<List<Order>> List()
        {
            return await ordersRepository.List();
        }

        public async Task<List<Order>> ListPaging(int pageIndex, int pageSize)
        {
            return await ordersRepository.ListPaging(pageIndex, pageSize);
        }
        public async Task<OrderPaging> ListPagingSort(OrderPagingRequest model)
        {
            return await ordersRepository.ListPagingSort(model);
        }
        public async Task<bool> UpdateOrderSort(UpdateOrder model)
        {
            return await ordersRepository.UpdateOrderSort(model);
        }
        public async Task<DTResult<OrdersViewModel>> ListServerSide(OrdersDTParameters parameters, int accountId)
        {
            var account = await accountRepository.Detail2(accountId);
            return await ordersRepository.ListServerSide(parameters, accountId, account.RoleId);
        }

        public async Task<List<Order>> Search(string keyword)
        {
            return await ordersRepository.Search(keyword);
        }

        public async Task Update(Order obj)
        {
            await ordersRepository.Update(obj);
        }
        public async Task UpdateByViewModel(Order order, OrderUpdateHistory orderUpdateHistory, bool confirmChange)
        {
            var database = ordersRepository.GetDatabase();
            using var transaction = database.BeginTransaction();
            try
            {
                await ordersRepository.UpdateByViewModel(order);
                if (confirmChange)
                {

                    await orderUpdateHistoryRepository.Add(orderUpdateHistory);
                }      
                await transaction.CommitAsync();
            }
            catch(Exception e)
            {            
                await transaction.RollbackAsync(); 
            }
            
        }
        public async Task<OrderStatusViewModel> GetTotalOrderStatus(int accountId)
        {
            var account = await accountRepository.Detail2(accountId);
            OrderStatusViewModel orderStatusViewModel = new OrderStatusViewModel();
            List<Order> totalOrderStatusViewModel = await ordersRepository.GetTotalOrderStatus();
            if(account.RoleId == RoleId.CONSUMER)
            {
                orderStatusViewModel.TotalNoProcess = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.NOPROCESS && x.AccountId == accountId).Count();
                orderStatusViewModel.TotalPurchaed = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.PURCHAED && x.AccountId == accountId).Count();
                orderStatusViewModel.TotalApproved = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.APPROVED && x.AccountId == accountId).Count();
                orderStatusViewModel.TotalDeposited = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.DEPOSITED && x.AccountId == accountId).Count();
                orderStatusViewModel.TotalCancel = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.CANCEL && x.AccountId == accountId).Count();
                orderStatusViewModel.TotalNoPurchaed = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.NOPURCHAED && x.AccountId == accountId).Count();
            }
            else
            {
                orderStatusViewModel.TotalNoProcess = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.NOPROCESS).Count();
                orderStatusViewModel.TotalPurchaed = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.PURCHAED).Count();
                orderStatusViewModel.TotalApproved = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.APPROVED).Count();
                orderStatusViewModel.TotalDeposited = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.DEPOSITED).Count();
                orderStatusViewModel.TotalCancel = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.CANCEL).Count();
                orderStatusViewModel.TotalNoPurchaed = totalOrderStatusViewModel.Where(x => x.OrderStatusId == OrderStatusId.NOPURCHAED).Count();
            }
            

            return orderStatusViewModel;
        }
        public async Task ChangeOrderStatus(Order model)
        {
            await ordersRepository.ChangeOrderStatus(model);
        }
        public async Task<List<OrdersViewModel>> ExportOrdersExcel(FillterExportOrderExcel fillter)
        {
            return await ordersRepository.ExportOrdersExcel(fillter);
        }
        public async Task<List<OrderSort>> GetAllProductDetail()
        {
            return await ordersRepository.GetAllProductDetail();
        }
        public async Task<OrderSort> GetDetailOderSort(int id)
        {
            return await ordersRepository.GetDetailOderSort(id);
        }
        public async Task<Order> GetOrderStatusAndFinal(int orderId)
        {
            return await ordersRepository.GetOrderStatusAndFinal(orderId);
        }
    }
}

