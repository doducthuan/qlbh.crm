
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
    public interface IProductRepository
    {
        Task<List<Product>> List();
        Task<List<Product>> ListLargeNumber();
        Task<List<Product>> Search(string keyword);

        Task<List<Product>> ListPaging(int pageIndex, int pageSize);

        Task<List<Product>> Detail(int? postId);

        Task<Product> Add(Product Product);

        Task Update(Product Product);

        Task Delete(Product Product);

        Task<int> DeletePermanently(int? ProductId);

        int Count();

        Task<DTResult<ProductViewModel>> ListServerSide(ProductDTParameters parameters);
    }
}
