using QLBH_Dion.Services;
using QLBH_Dion.Services.Interfaces;
using Quartz;

namespace QLBH_Dion.Quartz.Jobs
{
    public class AutoUpdateAuctionProduct : IJob
    {
        readonly IVPAProductService VPAProductService;
        public AutoUpdateAuctionProduct(IVPAProductService VPAProductService)
        {
            this.VPAProductService = VPAProductService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await VPAProductService.SynchronizedProduct();
        }
    }
}
