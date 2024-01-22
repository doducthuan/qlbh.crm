
        using QLBH_Dion.Models;
        using QLBH_Dion.Util;
        using QLBH_Dion.Util.Parameters;
        using QLBH_Dion.Models.ViewModels;
        using System.Threading.Tasks;
using QLBH_Dion.Models.ViewModel;

namespace QLBH_Dion.Services.Interfaces
{
            public interface IAuctionProductService : IBaseService<AuctionProduct>
            {
                Task<DTResult<AuctionProductViewModel>> ListServerSide(AuctionProductDTParameters parameters);
                Task<List<AuctionProductViewModel>> Detail2(int? id);
                Task<AuctionProductPaging> listPagingApp(AuctionProductPagingRequest model);
            }
        }
    