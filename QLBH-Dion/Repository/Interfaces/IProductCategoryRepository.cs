
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
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> List();

        Task<List<ProductCategory>> Search(string keyword);

        Task<List<ProductCategory>> ListPaging(int pageIndex, int pageSize);

        Task<List<ProductCategory>> Detail(int? postId);

        Task<ProductCategory> Add(ProductCategory ProductCategory);

        Task Update(ProductCategory ProductCategory);

        Task Delete(ProductCategory ProductCategory);

        Task<int> DeletePermanently(int? ProductCategoryId);

        int Count();

        Task<DTResult<ProductCategory>> ListServerSide(ProductCategoryDTParameters parameters);
    }
}
