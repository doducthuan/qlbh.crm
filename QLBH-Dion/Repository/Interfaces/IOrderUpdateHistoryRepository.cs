
using QLBH_Dion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace QLBH_Dion.Repository
{
    public interface IOrderUpdateHistoryRepository
    {
        Task<List<OrderUpdateHistory>> List();

        Task<List<OrderUpdateHistory>> Search(string keyword);

        Task<List<OrderUpdateHistory>> ListPaging(int pageIndex, int pageSize);

        Task<List<OrderUpdateHistory>> Detail(int? postId);

        Task<OrderUpdateHistory> Add(OrderUpdateHistory OrderUpdateHistory);

        Task Update(OrderUpdateHistory OrderUpdateHistory);

        Task Delete(OrderUpdateHistory OrderUpdateHistory);

        Task<int> DeletePermanently(int? OrderUpdateHistoryId);

        int Count();
        Task<List<OrderUpdateHistory>> GetChangeOrderNew(int orderId);
        DatabaseFacade GetDatabase();

        //Task<DTResult<OrderUpdateHistoryViewModel>> ListServerSide(OrderUpdateHistoryDTParameters parameters);
    }
}
