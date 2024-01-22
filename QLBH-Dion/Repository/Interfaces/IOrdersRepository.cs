
using QLBH_Dion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> List();

        Task<List<Order>> Search(string keyword);

        Task<List<Order>> ListPaging(int pageIndex, int pageSize);

        Task<List<Order>> Detail(int? postId);

        Task<Order> Add(Order Order);

        Task Update(Order Order);
        Task UpdateByViewModel(Order order);
        Task Delete(Order Order);

        Task<int> DeletePermanently(int? OrderId);

        int Count();
        Task<OrdersViewModel> Detail2(int orderId);
        Task<DTResult<OrdersViewModel>> ListServerSide(OrdersDTParameters parameters, int accountId, int accountRoleId);
        Task<OrderPaging> ListPagingSort(OrderPagingRequest model);
        Task<List<Order>> GetTotalOrderStatus();
        //Task<OrderPaging> ListPagingSort(string? keyword, int? orderStatusId, string? sortBy, string? sortType, int pageIndex, int pageSize);
        Task ChangeOrderStatus(Order model);
        Task<bool> UpdateOrderSort(UpdateOrder obj);
        Task<List<OrdersViewModel>> ExportOrdersExcel(FillterExportOrderExcel fillter);
        Task<List<OrderSort>> GetAllProductDetail();
        Task<OrderSort> GetDetailOderSort(int id);
        Task<Order> GetOrderStatusAndFinal(int orderId);
        DatabaseFacade GetDatabase();
    }
}
