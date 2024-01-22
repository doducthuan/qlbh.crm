using Serilog;
using QLBH_Dion.Repository;

namespace QLBH_Dion.Util.Entities
{
    public static class ApplicationExtensions
    {
        public static async void UseInfrastructure(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.Use(async (ctx, next) =>
                //{
                //    await next();
                //    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                //    {
                //        //Re-execute the request so the user gets the error page
                //        ctx.Request.Path = "/Pages404";
                //        await next();
                //    }
                //});
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.Use(async (ctx, next) =>
            //{
            //    await next();
            //    if (!ctx.Request.Path.Value.ToString().ToLower().Contains("api"))
            //    {
            //        if (ctx.Response.StatusCode == 401 && !ctx.Response.HasStarted)
            //        {
            //            ctx.Response.Redirect("/sign-in");
            //        }
            //    }
            //});
            //app.UseHttpsRedirection();

            app.Use(async (ctx, next) =>
            {
                await next();
                //if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                //{
                //    ctx.Response.Redirect("/page-not-found");
                //}
            });
            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            

            app.UseCors("Default");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseSerilogRequestLogging();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            //using (var scope = app.Services.CreateScope())
            //{
            //    // Get the DbContext instance
            //    var systemConfigRepository = scope.ServiceProvider.GetRequiredService<ISystemConfigRepository>();
            //    //var languageConfigRepository = scope.ServiceProvider.GetRequiredService<ILanguageConfigRepository>();
            //    var menuRepository = scope.ServiceProvider.GetRequiredService<IMenuRepository>();
            //    var rightsRepository = scope.ServiceProvider.GetRequiredService<IRightsRepository>();
            //    var roleRightsRepository = scope.ServiceProvider.GetRequiredService<IRoleRightsRepository>();
            //    var roleMenusRepository = scope.ServiceProvider.GetRequiredService<IRoleMenuRepository>();

            //    var cacheHelper = scope.ServiceProvider.GetRequiredService<ICacheHelper>();

            //    //Do the migration asynchronously
            //    var systemConfigs = await systemConfigRepository.List();
            //    cacheHelper.SetSystemConfig(systemConfigs);

            //    ////Do something with LanguageConfig
            //    //var languageConfigs = await languageConfigRepository.List();
            //    //cacheHelper.SetLanguageConfig(languageConfigs);

            //    ////Menu SystemAdmin
            //    var MenuByRole = await menuRepository.List();
            //    cacheHelper.SetMenuByRole(MenuByRole);
            //    ////Menu Auction_House_admin
            //    //var MenuAuctionHouseAdmin = await menuRepository.ListByRoleCode("AUCTION_HOUSE_ADMIN");
            //    //cacheHelper.SetMenuAuctionHouseAdmin(MenuAuctionHouseAdmin);
            //    ////Menu AuctioneerAdmin
            //    //var MenuAuctoneerAdmin = await menuRepository.ListByRoleCode("AUCTIONEER");
            //    //cacheHelper.SetMenuAuctioneerAdmin(MenuAuctoneerAdmin);
            //    ////Menu AuctioneerAdmin
            //    //var MenuAuctionUserSeller = await menuRepository.ListByRoleCode("AUCTION_OWNER");
            //    //cacheHelper.SetMenuUserSellerAdmin(MenuAuctionUserSeller);
            //    ////Menu Orthers AuctioneerAdmin
            //    //var MenuOrthersAuctioneerAdmin = await menuRepository.ListByRoleCode("ORTHERS_AUCTIONEER");
            //    //cacheHelper.SetMenuOrthersAuctioneer(MenuOrthersAuctioneerAdmin);
            //    ////Menu Auction_Assistant_Admin
            //    //var MenuAssistantAdmin = await menuRepository.ListByRoleCode("AUCTION_ASSISTANT");
            //    //cacheHelper.SetMenuAuctionAssistantAdmin(MenuAssistantAdmin);
            //    //List Rights
            //    var listRights = await rightsRepository.List();
            //    cacheHelper.SetRights(listRights);
            //    //List all Menu
            //    var listMenu = await menuRepository.List();
            //    cacheHelper.SetMenu(listMenu);
            //    //list roleRights not login
            //    var listRoleRightsNotLogin = await roleRightsRepository.ListByRoleId(SystemConstant.ROLE_ANONYMOUS_USER);
            //    cacheHelper.SetRoleRightsNotLogin(listRoleRightsNotLogin);
            //    //list roleRights all
            //    var listRoleRights = await roleRightsRepository.List();
            //    cacheHelper.SetRoleRights(listRoleRights);
            //    //list roleMenu all
            //    var listRoleMenus = await roleMenusRepository.List();
            //    cacheHelper.SetRoleMenu(listRoleMenus);
            //}
        }
    }
}
