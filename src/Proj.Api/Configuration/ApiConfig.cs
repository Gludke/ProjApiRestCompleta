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


                options.AddPolicy("Production",
                    builder =>
                        builder
                            .WithMethods("GET")//Somente essas requisições
                            .WithOrigins("http://desenvolvedor.io")//Somente dessas origens
                            .SetIsOriginAllowedToAllowWildcardSubdomains()//Permite apps rodando em sub-domínios dessa app principal
                            //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                            .AllowAnyHeader());
            });

            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            //Usando a política de acesso 'Development' configurada acima. Chamar sempre antes do 'UseMvc()' e das autorizações
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
            }
            else
            {
                app.UseCors("Production");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

    }
}
