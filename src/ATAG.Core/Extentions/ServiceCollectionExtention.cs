using ATAG.Core.Interfaces;
using ATAG.Core.Visitors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATAG.Core.Extentions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddAtagCore(this IServiceCollection services)
        {
            services.AddScoped<IMainParser, MainParser>();
            services.AddScoped(x => new MainVisitor());

            return services;
        }
    }
}
