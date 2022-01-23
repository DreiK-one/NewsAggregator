﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.Data
{

    //For auto-migration (update-database) ON START

    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using(var context = scope.ServiceProvider.GetRequiredService<NewsAggregatorContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception)
                    {
                        //Log errors or do anything it's needed
                        throw;
                    }
                }
            }

            return host;
        }
    }
}
