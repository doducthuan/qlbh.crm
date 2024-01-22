
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
    public interface IRoleRightRepository
    {
        Task<List<RoleRight>> List();

        Task<List<RoleRight>> Search(string keyword);

        Task<List<RoleRight>> ListPaging(int pageIndex, int pageSize);

        Task<List<RoleRight>> Detail(int? postId);

        Task<RoleRight> Add(RoleRight RoleRight);

        Task Update(RoleRight RoleRight);

        Task Delete(RoleRight RoleRight);

        Task<int> DeletePermanently(int? RoleRightId);

        int Count();

        Task<DTResult<RoleRightsViewModel>> ListServerSide(RoleRightsDTParameters parameters);
    }
}
