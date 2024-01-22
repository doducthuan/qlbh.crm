
        using QLBH_Dion.Models;
        using QLBH_Dion.Util;
        using QLBH_Dion.Util.Parameters;
        using QLBH_Dion.Models.ViewModels;
        using System.Threading.Tasks;
        
        namespace QLBH_Dion.Services.Interfaces
        {
            public interface IProductService : IBaseService<Product>
            {
                Task<DTResult<ProductViewModel>> ListServerSide(ProductDTParameters parameters);
                Task<List<Product>> ListLargeNumber();
            }
        }
    