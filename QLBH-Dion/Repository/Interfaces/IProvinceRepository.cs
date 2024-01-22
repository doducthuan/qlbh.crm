
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
    public interface IProvinceRepository
    {
        Task<List<Province>> List();

        Task<List<Province>> Search(string keyword);

        Task<List<Province>> ListPaging(int pageIndex, int pageSize);

        Task<List<Province>> Detail(int? postId);

        Task<Province> Add(Province Province);

        Task Update(Province Province);

        Task Delete(Province Province);

        Task<int> DeletePermanently(int? ProvinceId);

        int Count();

        //Task <DTResult<ProvinceViewModel>> ListServerSide(ProvinceDTParameters parameters);
    }
}
