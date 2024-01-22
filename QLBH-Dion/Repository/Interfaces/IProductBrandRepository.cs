using QLBH_Dion.Models;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Util;

namespace QLBH_Dion.Repository.Interfaces
{
    public interface IProductBrandRepository
    {
        Task<List<ProductBrand>> List();

        Task<List<ProductBrand>> Search(string keyword);

        Task<List<ProductBrand>> ListPaging(int pageIndex, int pageSize);

        Task<List<ProductBrand>> Detail(int? postId);

        Task<ProductBrand> Add(ProductBrand ProductBrand);

        Task Update(ProductBrand ProductBrand);

        Task Delete(ProductBrand ProductBrand);

        Task<int> DeletePermanently(int? ProductBrandId);

        int Count();

        Task<DTResult<ProductBrand>> ListServerSide(ProductBrandDTParameters parameters);
    }
}
