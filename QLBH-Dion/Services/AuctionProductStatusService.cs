
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
            public class AuctionProductStatusService : IAuctionProductStatusService
            {
                IAuctionProductStatusRepository auctionProductStatusRepository;
                public AuctionProductStatusService(
                    IAuctionProductStatusRepository _auctionProductStatusRepository
                    )
                {
                    auctionProductStatusRepository = _auctionProductStatusRepository;
                }
                public async Task Add(AuctionProductStatus obj)
                {
                    obj.Active = 1;
                    obj.CreatedTime = DateTime.Now;
                    await auctionProductStatusRepository.Add(obj);
                }
        
                public int Count()
                {
                    var result = auctionProductStatusRepository.Count();
                    return result;
                }
        
                public async Task Delete(AuctionProductStatus obj)
                {
                    obj.Active = 0;
                    await auctionProductStatusRepository.Delete(obj);
                }
        
                public async Task<int> DeletePermanently(int? id)
                {
                    return await auctionProductStatusRepository.DeletePermanently(id);
                }
        
                public async Task<List<AuctionProductStatus>> Detail(int? id)
                {
                    return await auctionProductStatusRepository.Detail(id);
                }
        
                public async Task<List<AuctionProductStatus>> List()
                {
                    return await auctionProductStatusRepository.List();
                }
        
                public async Task<List<AuctionProductStatus>> ListPaging(int pageIndex, int pageSize)
                {
                    return await auctionProductStatusRepository.ListPaging(pageIndex, pageSize);
                }
        
                public async Task<DTResult<AuctionProductStatus>> ListServerSide(AuctionProductStatusDTParameters parameters)
                {
                    return await auctionProductStatusRepository.ListServerSide(parameters);
                }
        
                public async Task<List<AuctionProductStatus>> Search(string keyword)
                {
                    return await auctionProductStatusRepository.Search(keyword);
                }
        
                public async Task Update(AuctionProductStatus obj)
                {
                    await auctionProductStatusRepository.Update(obj);
                }
            }
        }
    
    