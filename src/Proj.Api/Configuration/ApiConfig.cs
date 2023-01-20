using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Proj.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //Impede que a API faça as validações automáticas dela, dessa forma apenas as nossas
            //validações serão utilizadas, permitindo padronização de erros.
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            //Políticas de acesso à API
            services.AddCors(options =>
            {
                //Políticas de nome 'Development': acesso total atribuído nessas políticas
                options.AddPolicy("Development",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());


                //options.AddPolicy("Production",
                //    builder =>
                //        builder
                //            .WithMethods("GET")
                //            .WithOrigins("http://desenvolvedor.io")
                //            .SetIsOriginAllowedToAllowWildcardSubdomains()
                //            //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                //            .AllowAnyHeader());
            });

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //Usando a política de acesso 'Development' configurada acima
            app.UseCors("Development");

            return app;
        }

    }
}
