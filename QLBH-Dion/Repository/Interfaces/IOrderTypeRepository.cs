
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
    public interface IOrderTypeRepository
    {
        Task<List<OrderType>> List();

        Task<List<OrderType>> Search(string keyword);

        Task<List<OrderType>> ListPaging(int pageIndex, int pageSize);

        Task<List<OrderType>> Detail(int? postId);

        Task<OrderType> Add(OrderType OrderType);

        Task Update(OrderType OrderType);

        Task Delete(OrderType OrderType);

        Task<int> DeletePermanently(int? OrderTypeId);

        int Count();

        Task<DTResult<OrderType>> ListServerSide(OrderTypeDTParameters parameters);
    }
}
