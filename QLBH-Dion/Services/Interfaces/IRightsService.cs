
        using QLBH_Dion.Models;
        using QLBH_Dion.Util;
        using QLBH_Dion.Util.Parameters;
        using QLBH_Dion.Models.ViewModels;
        using System.Threading.Tasks;
        
        namespace QLBH_Dion.Services.Interfaces
        {
            public interface IRightsService : IBaseService<Right>
            {
                Task<DTResult<Right>> ListServerSide(RightsDTParameters parameters);
            }
        }
    