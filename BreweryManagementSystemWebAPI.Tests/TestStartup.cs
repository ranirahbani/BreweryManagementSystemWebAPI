using BreweryManagementSystemWebAPI.Data;
using BreweryManagementSystemWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagementSystemWebAPI.Tests
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("DefaultConnection"));

            // Register other services
            services.AddTransient<IBeerService, BeerService>();
            services.AddTransient<IWholesalerService, WholesalerService>();
            // Register additional services as needed
        }
    }

}
