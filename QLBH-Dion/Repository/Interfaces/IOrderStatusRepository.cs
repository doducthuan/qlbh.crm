
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
    public interface IOrderStatusRepository
    {
        Task<List<OrderStatus>> List();

        Task<List<OrderStatus>> Search(string keyword);

        Task<List<OrderStatus>> ListPaging(int pageIndex, int pageSize);

        Task<List<OrderStatus>> Detail(int? postId);

        Task<OrderStatus> Add(OrderStatus OrderStatus);

        Task Update(OrderStatus OrderStatus);

        Task Delete(OrderStatus OrderStatus);

        Task<int> DeletePermanently(int? OrderStatusId);

        int Count();

        Task<DTResult<OrderStatus>> ListServerSide(OrderStatusDTParameters parameters);
    }
}
