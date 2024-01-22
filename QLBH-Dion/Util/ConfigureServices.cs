using QLBH_Dion.Models;
using QLBH_Dion.Repository;
using QLBH_Dion.Services;
using QLBH_Dion.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using QLBH_Dion.Util.Provinces;
using QLBH_Dion.Repository.Interfaces;

namespace QLBH_Dion.Util
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<QLBHContext>(options => options.UseSqlServer(configuration.GetConnectionString("DBConnection"),
                builder => builder.MigrationsAssembly(typeof(QLBHContext).Assembly.FullName)));

            //services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Add repository
            #region
            //services.AddScoped<IEntityRepository, EntityRepository>();
            //services.AddScoped<IEntityService, EntityService>();

            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IAccountStatusRepository, AccountStatusRepository>();
            services.AddScoped<IAccountStatusService, AccountStatusService>();

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IAccountBuyRepository, AccountBuyRepository>();
            services.AddScoped<IAccountBuyService, AccountBuyService>();

            services.AddScoped<IOrderAccountService, OrderAccountService>();
            services.AddScoped<IOrderAccountRepository, OrderAccountRepository>();
            
            services.AddScoped<IOrderAccountPaymentStatusRepository, OrderAccountPaymentStatusRepository>();
            services.AddScoped<IOrderAccountPaymentStatusService, OrderAccountPaymentStatusService>();

            services.AddScoped<IOrderAccountStatusService, OrderAccountStatusService>();
            services.AddScoped<IOrderAccountStatusRepository, OrderAccountStatusRepository>();

            services.AddScoped<ITranslateProductService, TranslateProductService>();

            services.AddScoped<IRightsRepository, RightsRepository>();
            services.AddScoped<IRightsService, RightsService>();

            services.AddScoped<IRoleRightRepository, RoleRightRepository>();
            services.AddScoped<IRoleRightsService, RoleRightsService>();

            services.AddScoped<INotificationStatusRepository, NotificationStatusRepository>();
            services.AddScoped<INotificationStatusService, NotificationStatusService>();

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
            services.AddScoped<IActivityLogService, ActivityLogService>();

            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IProvinceService, ProvinceService>();
;
            services.AddScoped<ITokenService, TokenService>();
            //services.AddScoped<IProductRepository, ProductRepository>();
            //services.AddScoped<IProductRepository, OrderCallEventService>();

            services.AddScoped<IOrderRepository, OrdersRepository>();
            services.AddScoped<IOrdersService, OrdersService>();

            services.AddScoped<IOrderUpdateHistoryRepository, OrderUpdateHistoryRepository>();
            services.AddScoped<IOrderUpdateHistoryService, OrderUpdateHistoryService>();


            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            //Add scope Order

            services.AddScoped<IOrderTypeRepository, OrderTypeRepository>();
            services.AddScoped<IOrderTypeService, OrderTypeService>();

            services.AddScoped<ISettingRepository, SettingRepository>();
            services.AddScoped<ISettingService, SettingService>();

            services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
            services.AddScoped<IOrderStatusService, OrderStatusService>();

            //Add scope Product

            services.AddScoped<IAuctionProductRepository, AuctionProductRepository>();
            services.AddScoped<IAuctionProductService, AuctionProductService>();
            services.AddScoped<IProductBrandService, ProductBrandService>();
            services.AddScoped<IProductBrandRepository, ProductBrandRepository>();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IAuctionProductStatusService, AuctionProductStatusService>();
            services.AddScoped<IAuctionProductStatusRepository, AuctionProductStatusRepository>();
            #endregion
            //Add UploadFile
            //services.AddScoped<IFileExplorerService, FileExplorerService>();
            //services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IProvinceUtil, ProvinceUtil>();

            //services.AddScoped<INotificationHub, NotificationHub>();
            //services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            //Token service
            //services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IVPAProductService, VPAProductService>();
            
            return services;

        }
    }
}
