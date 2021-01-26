using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CicekSepeti.Api.Filters;
using CicekSepeti.Business.Concrete;
using CicekSepeti.Business.Helper;
using CicekSepeti.Business.Interfaces;
using CicekSepeti.Data.CacheService.Redis;
using CicekSepeti.Data.Concrete;
using CicekSepeti.Data.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;

namespace CicekSepeti.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("CicekSepetiSwagger", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CicekSepeti Basket Api",
                    Description = "An API to perform Basket operations"
                });
            });
            #endregion

            #region ModelState Error Disabled
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            #endregion

            #region json
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            #endregion

            services.Configure<CicekSepetiDatabaseSettings>(
               Configuration.GetSection(nameof(CicekSepetiDatabaseSettings)));

            services.AddSingleton<ICicekSepetiDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<CicekSepetiDatabaseSettings>>().Value);

            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            services.AddCors(o => o.AddPolicy("myclients", builder =>
            {
                builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
            }));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region AutoFac

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterType<BasketService>().As<IBasketService>().SingleInstance();
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<RedisServer>().SingleInstance();
            builder.RegisterType<RedisCacheService>().As<ICacheService>().SingleInstance();
            builder.RegisterType<BasketServiceHelper>().As<IBasketServiceHelper>().SingleInstance();
            builder.RegisterType<NotFoundFilter>();
            #endregion

            var appContainer = builder.Build();
            return new AutofacServiceProvider(appContainer);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("myclients");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/CicekSepetiSwagger/swagger.json", "CicekSepeti");
            });
        }

    }
}
