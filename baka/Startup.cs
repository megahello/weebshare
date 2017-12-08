using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http.Internal;

namespace baka
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            HttpLoggingService = new HttpLoggingService();
        }

        public IConfiguration Configuration { get; }
        public HttpLoggingService HttpLoggingService { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // services.Configure<FormFile>()
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                await next();

                string header = context.Request.Headers["baka_token"].FirstOrDefault();

                string ip = context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Connection.RemoteIpAddress.ToString();

                var request = new BakaRequest()
                {
                    DisplayUrl = context.Request.GetDisplayUrl(),
                    AuthHeader = header,
                    RemoteIp = ip,
                    Timestamp = DateTime.Now.ToFileTimeUtc().ToString(),
                    Method = context.Request.Method
                };

                await HttpLoggingService.Log(request);
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
