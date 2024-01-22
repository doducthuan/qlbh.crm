
        using QLBH_Dion.Models;
        using QLBH_Dion.Repository;
        using QLBH_Dion.Services.Interfaces;
        using QLBH_Dion.Util;
        using QLBH_Dion.Util.Parameters;
        using QLBH_Dion.Models.ViewModels;
        using System;
        using System.Collections.Generic;
        using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Services
        {
            public class AuctionProductService : IAuctionProductService
            {
                IAuctionProductRepository auctionProductRepository;
                public AuctionProductService(
                    IAuctionProductRepository _auctionProductRepository
                    )
                {
                    auctionProductRepository = _auctionProductRepository;
                }
                public async Task Add(AuctionProduct obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await auctionProductRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = auctionProductRepository.Count();
                    return result;
                }
        
                public async Task Delete(AuctionProduct obj)
                {
                    obj.Active = 0;
                    await auctionProductRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await auctionProductRepository.DeletePermanently(id);
                }
        
                public async Task<List<AuctionProduct>> Detail(int? id)
                {
                    return await auctionProductRepository.Detail(id);
                }

                public async Task<List<AuctionProductViewModel>> Detail2(int? id)
                {
                    return await auctionProductRepository.detail2(id);
                }

                public async Task<List<AuctionProduct>> List()
                {
                    return await auctionProductRepository.List();
                }
        
                public async Task<List<AuctionProduct>> ListPaging(int pageIndex, int pageSize)
                {
                    return await auctionProductRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<AuctionProductViewModel>> ListServerSide(AuctionProductDTParameters parameters)
                {
                    return await auctionProductRepository.ListServerSide(parameters);
                }
        
                public async Task<List<AuctionProduct>> Search(string keyword)
                {
                    return await auctionProductRepository.Search(keyword);
                }
        
                public async Task Update(AuctionProduct obj)
                {
                    await auctionProductRepository.Update(obj);
                }
                public async Task<AuctionProductPaging> listPagingApp(AuctionProductPagingRequest model)
                {
                    return await auctionProductRepository.ListPagingApp(model);
                }
            }
        }
    
    