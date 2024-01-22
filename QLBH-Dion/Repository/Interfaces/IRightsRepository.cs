
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
    public interface IRightsRepository
    {
        Task<List<Right>> List();

        Task<List<Right>> Search(string keyword);

        Task<List<Right>> ListPaging(int pageIndex, int pageSize);

        Task<List<Right>> Detail(int? postId);

        Task<Right> Add(Right Rights);

        Task Update(Right Rights);

        Task Delete(Right Rights);

        Task<int> DeletePermanently(int? RightsId);

        int Count();

        Task<DTResult<Right>> ListServerSide(RightsDTParameters parameters);
    }
}
