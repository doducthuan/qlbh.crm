
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
    public interface IRoleRepository
    {
        Task<List<Role>> List();

        Task<List<Role>> Search(string keyword);

        Task<List<Role>> ListPaging(int pageIndex, int pageSize);

        Task<List<Role>> Detail(int? postId);

        Task<Role> Add(Role Role);

        Task Update(Role Role);

        Task Delete(Role Role);

        Task<int> DeletePermanently(int? RoleId);

        int Count();

        Task<DTResult<Role>> ListServerSide(RoleDTParameters parameters);
    }
}
