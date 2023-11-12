using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainWave.Infrastructure.Data;

namespace BrainWave.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static void AddStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<BrainWaveDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("BrainWaveDbContext"));
            });
        }
    }
}
