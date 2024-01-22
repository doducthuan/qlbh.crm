
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
            public class ProductCategoryService : IProductCategoryService
            {
                IProductCategoryRepository productCategoryRepository;
                public ProductCategoryService(
                    IProductCategoryRepository _productCategoryRepository
                    )
                {
                    productCategoryRepository = _productCategoryRepository;
                }
                public async Task Add(ProductCategory obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await productCategoryRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = productCategoryRepository.Count();
                    return result;
                }
        
                public async Task Delete(ProductCategory obj)
                {
                    obj.Active = 0;
                    await productCategoryRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await productCategoryRepository.DeletePermanently(id);
                }
        
                public async Task<List<ProductCategory>> Detail(int? id)
                {
                    return await productCategoryRepository.Detail(id);
                }
        
                public async Task<List<ProductCategory>> List()
                {
                    return await productCategoryRepository.List();
                }
        
                public async Task<List<ProductCategory>> ListPaging(int pageIndex, int pageSize)
                {
                    return await productCategoryRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<ProductCategory>> ListServerSide(ProductCategoryDTParameters parameters)
                {
                    return await productCategoryRepository.ListServerSide(parameters);
                }
        
                public async Task<List<ProductCategory>> Search(string keyword)
                {
                    return await productCategoryRepository.Search(keyword);
                }
        
                public async Task Update(ProductCategory obj)
                {
                    await productCategoryRepository.Update(obj);
                }
            }
        }
    
    