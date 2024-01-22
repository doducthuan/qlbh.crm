
using QLBH_Dion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Repository
{
    public interface IAuctionProductRepository
    {
        Task<List<AuctionProduct>> List();

        Task<List<AuctionProduct>> Search(string keyword);

        Task<List<AuctionProduct>> ListPaging(int pageIndex, int pageSize);

        Task<List<AuctionProduct>> Detail(int? postId);

        Task<AuctionProduct> Add(AuctionProduct AuctionProduct);
        Task AddRange(List<AuctionProduct> AuctionProduct);
        Task UpdateRange(List<AuctionProduct> products);

        Task Update(AuctionProduct AuctionProduct);

        Task Delete(AuctionProduct AuctionProduct);

        Task<int> DeletePermanently(int? AuctionProductId);

        int Count();
        Task<List<AuctionProductViewModel>> detail2 (int? id);
        Task<DTResult<AuctionProductViewModel>> ListServerSide(AuctionProductDTParameters parameters);
        Task<List<AuctionProduct>> ListByProductId(int productId);
        DatabaseFacade GetDatabase();
        Task<AuctionProductPaging> ListPagingApp(AuctionProductPagingRequest model);
    }
}
