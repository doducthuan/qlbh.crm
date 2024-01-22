using QLBH_Dion.Models;
using QLBH_Dion.Repository;
using QLBH_Dion.Repository.Interfaces;
using QLBH_Dion.Services.Interfaces;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;

namespace QLBH_Dion.Services
{
    public class ProductBrandService : IProductBrandService
    {
        IProductBrandRepository productBrandRepository;
        public ProductBrandService(IProductBrandRepository _productBrandRepository)
        {
            productBrandRepository = _productBrandRepository;
        }
        public async Task Add(ProductBrand obj)
        {
            obj.Active = 1;
            obj.CreatedTime = DateTime.Now;
            await productBrandRepository.Add(obj);
        }

        public int Count()
        {
            return productBrandRepository.Count();
        }

        public async Task Delete(ProductBrand obj)
        {
            obj.Active = 0;
            await productBrandRepository.Delete(obj);
        }

        public async Task<int> DeletePermanently(int? id)
        {
            return await productBrandRepository.DeletePermanently(id);
        }

        public async Task<List<ProductBrand>> Detail(int? id)
        {
            return await productBrandRepository.Detail(id);
        }

        public async Task<List<ProductBrand>> List()
        {
            return await productBrandRepository.List();
        }

        public async Task<List<ProductBrand>> ListPaging(int pageIndex, int pageSize)
        {
            return await productBrandRepository.ListPaging(pageIndex, pageSize);
        }

        public async Task<DTResult<ProductBrand>> ListServerSide(ProductBrandDTParameters parameters)
        {
            return await productBrandRepository.ListServerSide(parameters);
        }

        public async Task<List<ProductBrand>> Search(string keyword)
        {
            return await productBrandRepository.Search(keyword);
        }

        public async Task Update(ProductBrand obj)
        {
            await productBrandRepository.Update(obj);
        }
    }
}
