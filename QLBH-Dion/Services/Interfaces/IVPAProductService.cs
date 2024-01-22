using QLBH_Dion.Util.VPAProduct;

namespace QLBH_Dion.Services.Interfaces
{
    public interface IVPAProductService
    {
        Task<VPAProductResponse> GetProductByPageAndStatus(VPAProductRequest request);
        Task<bool> SynchronizedProduct();
    }
}
