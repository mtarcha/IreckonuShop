using System.IO;
using AutoMapper;
using IreckonuShop.BusinessLogic.Services;
using IreckonuShop.Common.Utilities.HashCalculation;
using IreckonuShop.Common.Utilities.Serialization;
using IreckonuShop.Domain;
using IreckonuShop.PersistenceLayer.FileSystem;
using IreckonuShop.PersistenceLayer.RelationalDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IreckonuShop.API
{
    public class Startup
    {
        // todo: load same products!
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var storage = Configuration["StorageType"];
            var connectionString = Configuration.GetConnectionString("IreckonuStorageConnectionString");
            if (storage == "sql")
            {
                services.AddDbContext<IreckonuShopDbContext>(cfg =>
                {
                    cfg.UseSqlServer(connectionString);
                });

                services.AddScoped<IProductsRepository, SqlProductsRepository>();
            }
            else if(storage == "json")
            {
                var fileSystemProductsRepository = new FileSystemProductsRepository(
                    connectionString, 
                    new System.IO.Abstractions.FileSystem(), 
                    new JsonSerializer<Product>(),
                    new Sha256HashCalculator());

                services.AddScoped<IProductsRepository, FileSystemProductsRepository>(x => fileSystemProductsRepository);
            }
            else
            {
                throw new InvalidDataException("Unknown storage type.");
            }

            services.AddScoped<ILocalizedStringParser, DutchParser>();
            services.AddScoped<IProductService, ProductService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(new Profile[]
                {
                    new Utilities.AutoMapper(),
                });
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsProduction())
            {
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
