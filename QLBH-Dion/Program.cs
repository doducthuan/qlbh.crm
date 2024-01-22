using QLBH_Dion.Util.Entities;
using QLBH_Dion.Util;
using Serilog;
using QLBH_Dion.Services;
using Quartz;
using QLBH_Dion.Quartz.Jobs;
var builder = WebApplication.CreateBuilder(args);

// Add seri log
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .CreateLogger();
Log.Information($"Start {builder.Environment.ApplicationName} up.");

try
{
    // Add services to the container.
    builder.Services
           .AddInfrastructure(builder.Configuration)
           .AddInfrastructureServices(builder.Configuration)
           .AddSignalR();
    builder.Host.UseSerilog();

    //Add Quartz to the container
    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        var jobKey = new JobKey("CheckExpiredOrdersJob");
        q.AddJob<AutoUpdateAuctionProduct>(opt => opt.WithIdentity(jobKey));
        q.AddTrigger(opt => opt.ForJob(jobKey).WithIdentity("TriggerForUpdateAuctionProduct").WithCronSchedule("0 0 */3 ? * *"));
        //q.AddTrigger(opt => opt.ForJob(jobKey).WithIdentity("TriggerForUpdateAuctionProduct").WithCronSchedule("0 30 23 ? * * *"));
        //q.AddTrigger(opt => opt.ForJob(jobKey).WithIdentity("TriggerUpdateAuctionProduct").WithCronSchedule("* * * ? * *"));
    });
    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    builder.Configuration.AddJsonFile("appsettings.json", true, true).AddEnvironmentVariables();
    AppSettingConfig.Instance.SetConfiguration(builder.Configuration);
    var app = builder.Build();
    app.UseInfrastructure();
    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
    Log.Fatal(ex, $"Unhanded exception: {ex.Message}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete.");
    Log.CloseAndFlush();
}

