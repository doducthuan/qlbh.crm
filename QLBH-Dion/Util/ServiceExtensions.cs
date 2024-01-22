using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NPOI.HSSF.Record.Chart;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace QLBH_Dion.Util
{
    public static class ServiceExtensions
    {
        private static readonly HttpClient httpClient;
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllersWithViews().AddNewtonsoftJson().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();
            services.AddSingleton<ICacheHelper, CacheHelper>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddHttpContextAccessor();
            services.AddControllersWithViews()
                .ConfigureApiBehaviorOptions(opt =>
                {
                    opt.SuppressModelStateInvalidFilter = true;
                })
                .AddSessionStateTempDataProvider();
            services.AddCors(options =>
            {
                var origins = configuration.GetSection("CorsOrigins").Get<string[]>();
                options.AddPolicy("Default", policy =>
                {
                    policy.WithOrigins(configuration.GetSection("CorsOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
                    //policy.AllowAnyOrigin().AllowAnyHeader().AllowCredentials().AllowAnyMethod();
                });
            });
            //ADD SWWAGGER 
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HAPPYS API", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                    ValidAudience = configuration.GetValue<string>("Jwt:Issuer"),
                    IssuerSigningKey = new
                        SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes
                            (configuration.GetValue<string>("Jwt:Key")))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (!context.Request.Path.Value.Contains("api"))
                        {
                            context.Token = context.Request.Cookies["Authorization"];
                        }
                        else
                        {
                            var accessToken = context.Request.Query["access_token"];
                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                               (path.StartsWithSegments("/AccountSendMessageHub")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            //services.AddAuthorization(options =>
            //{
            //    options.DefaultPolicy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .AddAuthenticationSchemes("Bearer")
            //        .Build();
            //});
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            //HTTPS ENFORCE
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });
            return services;
        }
    }
}
