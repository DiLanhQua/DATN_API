 using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace DATN_API.Extension
{
    public static class APIRequestration
    {
        public static IServiceCollection AddApiReguestration(this IServiceCollection services)
        {

            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //FileProvide
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot"
                )));



            //Enable CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", pol =>
                {
                    pol.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000");
                });
            });
            return services;
        }
    }
}
