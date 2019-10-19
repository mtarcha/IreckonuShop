using AutoMapper;
using CsvHelper.Configuration;
using IreckonuShop.BusinessLogic.Services;
using IreckonuShop.Domain;
using IreckonuShop.PersistenceLayer.Relational;
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
            if (storage == "sql")
            {
                var connectionString = Configuration.GetConnectionString("IreckonuDBConnectionString");
                services.AddDbContext<IreckonuShopDbContext>(cfg =>
                {
                    cfg.UseSqlServer(connectionString);
                });

                services.AddScoped<IProductsRepository, SqlProductsRepository>();
            }
            else if(storage == "json")
            {
                
            }
            else
            {
                throw new ConfigurationException("Unknown storage.");
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
