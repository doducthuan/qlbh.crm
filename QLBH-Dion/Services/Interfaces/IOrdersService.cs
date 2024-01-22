
using QLBH_Dion.Models;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace QLBH_Dion.Services.Interfaces
{
    public interface IOrdersService : IBaseService<Order>
    {
        Task<DTResult<OrdersViewModel>> ListServerSide(OrdersDTParameters parameters, int accountId);
        Task<OrdersViewModel> Detail2(int orderId);
        Task UpdateByViewModel(Order order, OrderUpdateHistory orderUpdateHistory, bool confirmChange);
        Task<OrderStatusViewModel> GetTotalOrderStatus(int accountId);
        Task<OrderPaging> ListPagingSort(OrderPagingRequest model);
        Task<bool> UpdateOrderSort(UpdateOrder model);
        Task ChangeOrderStatus(Order model);
        Task<List<OrdersViewModel>> ExportOrdersExcel(FillterExportOrderExcel fillter);
        Task<List<OrderSort>> GetAllProductDetail();
        Task<OrderSort> GetDetailOderSort(int id);
        Task<Order> GetOrderStatusAndFinal(int orderId);
    }
}
