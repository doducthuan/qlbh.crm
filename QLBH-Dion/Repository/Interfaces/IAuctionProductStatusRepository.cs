
using QLBH_Dion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLBH_Dion.Util;
using QLBH_Dion.Util.Parameters;
using QLBH_Dion.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace QLBH_Dion.Repository
{
    public interface IAuctionProductStatusRepository
    {
        Task<List<AuctionProductStatus>> List();

        Task<List<AuctionProductStatus>> Search(string keyword);

        Task<List<AuctionProductStatus>> ListPaging(int pageIndex, int pageSize);

        Task<List<AuctionProductStatus>> Detail(int? postId);

        Task<AuctionProductStatus> Add(AuctionProductStatus AuctionProductStatus);

        Task Update(AuctionProductStatus AuctionProductStatus);

        Task Delete(AuctionProductStatus AuctionProductStatus);

        Task<int> DeletePermanently(int? AuctionProductStatusId);

        int Count();

        Task<DTResult<AuctionProductStatus>> ListServerSide(AuctionProductStatusDTParameters parameters);
    }
}
