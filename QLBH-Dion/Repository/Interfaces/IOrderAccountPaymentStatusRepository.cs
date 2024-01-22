
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
    public interface IOrderAccountPaymentStatusRepository
    {
        Task<List<OrderAccountPaymentStatus>> List();

        Task<List<OrderAccountPaymentStatus>> Search(string keyword);

        Task<List<OrderAccountPaymentStatus>> ListPaging(int pageIndex, int pageSize);

        Task<List<OrderAccountPaymentStatus>> Detail(int? postId);

        Task<OrderAccountPaymentStatus> Add(OrderAccountPaymentStatus OrderAccountPaymentStatus);

        Task Update(OrderAccountPaymentStatus OrderAccountPaymentStatus);

        Task Delete(OrderAccountPaymentStatus OrderAccountPaymentStatus);

        Task<int> DeletePermanently(int? OrderAccountPaymentStatusId);

        int Count();

        Task<DTResult<OrderAccountPaymentStatus>> ListServerSide(OrderAccountPaymentStatusDTParameters parameters);
    }
}
