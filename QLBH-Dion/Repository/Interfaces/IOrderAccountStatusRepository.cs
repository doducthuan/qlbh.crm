
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
    public interface IOrderAccountStatusRepository
    {
        Task<List<OrderAccountStatus>> List();

        Task<List<OrderAccountStatus>> Search(string keyword);

        Task<List<OrderAccountStatus>> ListPaging(int pageIndex, int pageSize);

        Task<List<OrderAccountStatus>> Detail(int? postId);

        Task<OrderAccountStatus> Add(OrderAccountStatus OrderAccountStatus);

        Task Update(OrderAccountStatus OrderAccountStatus);

        Task Delete(OrderAccountStatus OrderAccountStatus);

        Task<int> DeletePermanently(int? OrderAccountStatusId);

        int Count();

        Task<DTResult<OrderAccountStatus>> ListServerSide(OrderAccountStatusDTParameters parameters);
    }
}
