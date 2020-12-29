using EventBooking.Api.Filters;
using EventBooking.Model;
using EventBooking.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string eventBookingCorsPolicy = "EventBookingCorsPolicy";


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventBooking.Api", Version = "v1" });
            });


            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["ApiKey"]));
            var jwtAppSettingOptions = Configuration.GetSection("JwtIssuerOptions");
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<MvJwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(MvJwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(MvJwtIssuerOptions.Audience)];
                options.ValidFor = Convert.ToDouble(jwtAppSettingOptions[nameof(MvJwtIssuerOptions.ValidFor)]);
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha512Signature);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(MvJwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddControllers()
                .AddMvcOptions(o =>
                {
                    o.Filters.Add(new ValidateModelAttribute());
                })
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    //o.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ssZ";
                });

            var allowOrigin = Configuration.GetSection("AllowedOrigins").Get<List<string>>(); // Allow Origins for address added in Configuration>AllowOrigin
            services.AddCors(options =>
            {
                options.AddPolicy(eventBookingCorsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(allowOrigin.ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            //var origins = Configuration.GetSection("AllowedOrigins").Get<List<string>>();
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(eventBookingCorsPolicy,
            //         b => //b.WithOrigins(origins.ToArray())
            //        b.AllowAnyOrigin()
            //        );
            //});

            services
               .AddTransient<IBaseService, BaseService>()
               .AddTransient<IJwtFactory, JwtFactory>()
               .AddTransient<IPersonService, PersonService>()
               .AddTransient<IAccountService, AccountService>()
               .AddTransient<IEventService, EventService>()
               .AddTransient<IBookingService, BookingService>()
               .AddSingleton<IUtilityService, UtilityService>()
               .AddScoped<IDataAccessService, DataAccessService>()
               .AddSingleton(Configuration)
               .AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventBooking.Api v1"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseResponseCompression();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(eventBookingCorsPolicy);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(eventBookingCorsPolicy);
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Unauthorized");
            });
        }
    }
}
