using QLBH_Dion.Models;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Util;

namespace QLBH_Dion.Services.Interfaces
{
    public interface IProductBrandService : IBaseService<ProductBrand>
    {
        Task<DTResult<ProductBrand>> ListServerSide(ProductBrandDTParameters parameters);
    }
}
