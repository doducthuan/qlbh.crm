
        using QLBH_Dion.Models;
        using QLBH_Dion.Repository;
        using QLBH_Dion.Services.Interfaces;
        using QLBH_Dion.Util;
        using QLBH_Dion.Util.Parameters;
        using QLBH_Dion.Models.ViewModels;
        using System;
        using System.Collections.Generic;
        using System.Threading.Tasks;
        
        namespace QLBH_Dion.Services
        {
            public class ProductService : IProductService
            {
                IProductRepository productRepository;
                public ProductService(
                    IProductRepository _productRepository
                    )
                {
                    productRepository = _productRepository;
                }
                public async Task Add(Product obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await productRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = productRepository.Count();
                    return result;
                }
        
                public async Task Delete(Product obj)
                {
                    obj.Active = 0;
                    await productRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await productRepository.DeletePermanently(id);
                }
        
                public async Task<List<Product>> Detail(int? id)
                {
                    return await productRepository.Detail(id);
                }
        
                public async Task<List<Product>> List()
                {
                    return await productRepository.List();
                }

                public async Task<List<Product>> ListLargeNumber()
                {
                    return await productRepository.ListLargeNumber();
                }

                public async Task<List<Product>> ListPaging(int pageIndex, int pageSize)
                {
                    return await productRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<ProductViewModel>> ListServerSide(ProductDTParameters parameters)
                {
                    return await productRepository.ListServerSide(parameters);
                }
        
                public async Task<List<Product>> Search(string keyword)
                {
                    return await productRepository.Search(keyword);
                }
        
                public async Task Update(Product obj)
                {
                    await productRepository.Update(obj);
                }
            }
        }
    
    